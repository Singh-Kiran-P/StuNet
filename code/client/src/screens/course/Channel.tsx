import React, { Screen, Message, useEffect, useState, useToken, jwt_decode, axios, display, timeDiff } from '@/.';
import { Text, List, Loader, SearchBar, CompactMessage } from '@/components';
import { useConnection } from '@/connection';

export default Screen('Channel', ({ nav, params: { id, name } }) => {
	let [messages, setMessages] = useState<Message[]>([]);
	let [error, setError] = useState('');

	const connection = useConnection();
	let email = (jwt_decode(useToken()[0]) as any).username as string;

	let lastTime = '';
	let time = (current: Message, previous?: Message) => {
		let user = previous?.userMail !== current.userMail;
		let diff = timeDiff(lastTime, current.time) >= 60 * 5;
		let time = user || diff;
		if (time) lastTime = current.time;
		return time;
	}

	useEffect(() => {
		connection.invoke('JoinChannel', id).catch(display(setError));
		connection.on('messageReceived', (username: string, message: string, time: string) => {
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
	}, [])

	const fetch = async () => {
		return axios.get('/Channel/' + id).then(res => {
			setMessages(res.data.messages.reverse());
			nav.setParams({ course: res.data.course?.name, name: res.data.name })
		})
	}

    const sendMessage = (msg: string) => {
		connection.invoke('SendMessageToChannel', msg, id).catch(display(setError));
	}

	return (
		<Loader load={fetch}>
			<List flex inner padding inverted data={messages} renderItem={({ item, index }) => (
				<CompactMessage margin='bottom' message={item} sender={item.userMail === email}
					time={time(item, messages[index - 1])}
				/>
			)}/>
			<Text type='error' pad margin='bottom' hidden={!error} children={error}/>
			<SearchBar pad='bottom' icon='send' returnKeyType='send' placeholder={'Message ' + name} disableEmpty
				onSearch={(msg, set) => (set(''), sendMessage(msg))}
			/>
		</Loader>
	)
})
