import React, { Screen, Message, useEffect, useState, useToken, jwt_decode, axios, display } from '@/.';
import { Text, List, Loader, SearchBar, CompactMessage } from '@/components';
import { useConnection } from '@/connection';

export default Screen('Channel', ({ nav, params: { id, name } }) => {
	let [messages, setMessages] = useState<Message[]>([]);
	let [error, setError] = useState('');
	
	const connection = useConnection();
	let email = (jwt_decode(useToken()[0]) as any).username as string;

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
			<List flex inner padding='vertical' inverted data={messages} renderItem={message => (
				<CompactMessage margin='bottom' message={message.item} sender={message.item.userMail === email}/>
			)}/>
			<Text type='error' margin='bottom' hidden={!error} children={error}/>
			<SearchBar icon='send' returnKeyType='send' placeholder={'Message ' + name} disableEmpty
				onSearch={(msg, set) => (set(''), sendMessage(msg))}
			/>
		</Loader>
	)
})
