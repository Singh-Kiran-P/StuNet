import React, { Screen, Message, useEffect, useState, useEmail, axios, update, display, timeDiff } from '@/.';
import { Text, List, Loader, SearchBar, CompactMessage } from '@/components';
import { useConnection } from '@/connection';

export default Screen('Channel', ({ nav, params: { id, name } }) => {
	let [messages, setMessages] = useState<Message[]>([]);
	let [courseId, setCourseId] = useState(NaN);
	let [error, setError] = useState('');

	const connection = useConnection();
	let email = useEmail();

	let first = (current: Message, previous?: Message) => {
		if (previous?.userMail !== current.userMail) return true;
		let time = lastTime;
		let first = !!previous && last(previous, current);
		lastTime = time;
		return first;
	}

	let lastTime = '';
	let last = (current: Message, next?: Message) => {
		let user = next?.userMail !== current.userMail;
		let diff = timeDiff(lastTime, current.time) >= 60 * 5;
		let last = user || diff;
		if (last) lastTime = current.time;
		return last;
	}

	useEffect(() => {
		if (isNaN(courseId)) return;
		connection.invoke('JoinChannel', id).catch(display(setError));
		connection.on('messageReceived', (username: string, message: string, time: string) => {
			update('Course', { id: courseId });
			setMessages(messages => [{
				userMail: username,
                body: message,
                time: time
			}, ...messages, ]);
		})

		return () => {
			connection.off('messageReceived');
			connection.invoke('LeaveChannel', id).catch(display(setError));
		}
	}, [courseId])

	const fetch = async () => {
		return axios.get('/Channel/' + id).then(res => {
			setCourseId(res.data.course?.id);
			setMessages(res.data.messages.reverse());
			nav.setParams({ course: res.data.course?.name, name: res.data.name })
		})
	}

    const sendMessage = (msg: string) => {
		connection.invoke('SendMessageToChannel', msg, id).catch(display(setError));
	}

	return (
		<Loader load={fetch}>
			<List flex inner padding inverted data={messages} ListFooterComponent={
				<Text type='hint' margin='bottom-2' children={`This is the start of the ${name} channel`}/>
			} renderItem={({ item, index }) => (
				<CompactMessage margin='bottom' message={item} sender={item.userMail === email}
					last={last(item, messages[index - 1])}
					first={first(item, messages[index + 1])}
				/>
			)}/>
			<Text type='error' pad margin='bottom' hidden={!error} children={error}/>
			<SearchBar pad='bottom' icon='send' returnKeyType='send' placeholder={'Message ' + name} disableEmpty
				onSearch={(msg, set) => (set(''), sendMessage(msg))}
			/>
		</Loader>
	)
})
