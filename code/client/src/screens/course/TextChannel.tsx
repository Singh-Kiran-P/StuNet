import React, { Screen, useTheme, extend } from '@/.';
import {  View, TextInput, Text, Loader } from '@/components'
import { useEffect, useRef, useState } from 'react';
import { FlatList } from 'react-native';
import { useConnection } from '@/connection';
import { useToken } from '@/.';
import axios from 'axios';
import jwt_decode from 'jwt-decode';
import moment from 'moment'

enum Alignment { Left, Right };

type Message = {
	userMail: string,
	body: string,
	time: string,
}

type Props = {
	user: string
	color: string,
	time: string,
	alignment: Alignment,
};

const prettyPrintDateTime = (date: Date) => {
	const today = new Date()
	if (today.getUTCFullYear() == date.getUTCFullYear()) {
		if (today.getUTCMonth() == date.getUTCMonth()) {
			if (today.getUTCDay() == date.getUTCDay()) {
				return moment(date).local().format('HH:mm')
			} else {
				return moment(date).local().format('D MMM, HH:mm')
			}
		}
	} else {
		return moment(date).local().format('D MMM yyyy, HH:mm')
	}
}

const Message = extend<typeof View, Props>(View, ({ user, color, time, alignment, children }) => {
	const align = alignment == Alignment.Left ? 'flex-start' : 'flex-end'
	const textAlign = alignment == Alignment.Left ? 'left' : 'right'

	return <View style={{ flex: 1, maxWidth: '70%', backgroundColor: color, borderRadius: 10, padding: 10, marginTop: 5, alignSelf: align, alignItems: align }}>
		<Text style={{ fontWeight: 'bold', textAlign: textAlign }}>{user}</Text>
		<Text style={{ textAlign: textAlign }}>{children}</Text>
		<Text style={{ alignSelf: alignment == Alignment.Left ? 'flex-end' : 'flex-start' }}>{prettyPrintDateTime(new Date(time))}</Text>
	</View>
});

export default Screen('TextChannel', ({ params, nav }) => {
	const [message, setMessage] = useState('');
	const [messages, setMessages] = useState<Message[]>([]);
	const [newMessage, setNewMessage] = useState<Message>();
	const connection = useConnection();
	const email = jwt_decode(useToken()[0]).username;

	let [theme] = useTheme();
	let listRef = useRef();

	useEffect(() => {
		if (newMessage) {
			setMessages([...messages, newMessage])
		}
	}, [newMessage])

	useEffect(() => {
		connection.invoke('JoinChannel', params.channel.id)

		connection.on('messageReceived', (username: string, message: string, time: string) => {
			setNewMessage({
				userMail: username,
                body: message,
                time: time
			})
		});

		return () => {
			connection.off('messageReceived')
			connection.invoke('LeaveChannel', params.channel.id)
		}

	}, []);

	const fetch = () => {
		return axios.get('/Channel/' + params.channel.id)
			.then(res => {
				setMessages(res.data.messages)
				nav.setParams({ screenTitle: '{course}: #{channelName}', channelName: params.channel.name })
			});
	}

    const sendMessage = (msg: string) => {
		connection.invoke('SendMessageToChannel', msg, params.channel.id)
            .catch(err => console.log(err));

		setMessage('');
	}

	return (
		<Loader load={fetch} style={{ flex: 1 }}>
			<FlatList ref={listRef} contentContainerStyle={{ flexGrow: 1, justifyContent: 'flex-end' }} data={messages} onContentSizeChange={() => listRef.current.scrollToEnd()} renderItem={
				({ item, index }) => (
					<Message key={index} user={item.userMail} time={item.time} color={item.userMail === email ? theme.primary : theme.surface} alignment={item.userMail === email ? Alignment.Right : Alignment.Left}>
						{item.body}
					</Message>
				)} />

			<TextInput value={message} placeholder={'send message in #' + params.channel.name} onChangeText={setMessage} onSubmitEditing={(e) => sendMessage(message)} returnKeyType='send' />
		</Loader>
	)
})
