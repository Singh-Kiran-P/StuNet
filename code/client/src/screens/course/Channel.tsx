import React, { Screen, Message, useEffect, useState, useToken, jwt_decode, axios, show } from '@/.';
import { Text, List, Loader, SearchBar, CompactMessage } from '@/components';
import { useConnection } from '@/connection';

export default Screen('Channel', ({ params: { id, name }, nav }) => {
	let [messages, setMessages] = useState<Message[]>([]);
	let [error, setError] = useState('');
	
	const connection = useConnection();
	let email = (jwt_decode(useToken()[0]) as any).username as string;

	useEffect(() => {
		connection.invoke('JoinChannel', id);
		connection.on('messageReceived', (username: string, message: string, time: string) => {
			setMessages(messages => [...messages, {
				userMail: username,
                body: message,
                time: time
			}])
		})

		return () => {
			connection.off('messageReceived');
			connection.invoke('LeaveChannel', id);
		}
	}, [])

	const fetch = async () => {
		return axios.get('/Channel/' + id).then(res => {
			setMessages(res.data.messages);
			nav.setParams({ course: 'TODO GET COURSE', name: res.data.name })
		})
	}

    const sendMessage = (msg: string) => {
		connection.invoke('SendMessageToChannel', msg, id).catch(show(setError));
	}

	return (
		<Loader load={fetch}>
			<List flex content padding='bottom' inverted data={messages} renderItem={message => (
				<CompactMessage margin='bottom' message={message.item} sender={message.item.userMail === email}/>
			)}/>
			<Text type='error' margin hidden={!error} children={error}/>
			<SearchBar icon='send' placeholder={'Message ' + name} disableEmpty onSearch={(msg, set) => (set(''), sendMessage(msg))}/>
		</Loader>
	)
})
