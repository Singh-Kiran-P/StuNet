import React, { Screen, useTheme, extend } from "@/.";
import {  View, TextInput, Text, Loader } from '@/components'
import { useEffect, useRef, useState } from "react";
import { FlatList } from "react-native";
import { useConnection } from "@/connection";
import { useToken } from "@/.";
import axios from "axios";
import jwt_decode from "jwt-decode";
import moment from 'moment'

enum Alignment { Left, Right };

type Message = {
	userMail: string,
	body: string,
	dateTime: string,
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
				return moment(date).format('HH:mm')
			} else {
				return moment(date).format('D MMM, HH:mm')
			}
		}
	} else {
		return moment(date).format('D MMM yyyy, HH:mm')
	}
}

const Message = extend<typeof View, Props>(View, ({ user, color, time, alignment, children }) => {
	const align: string = alignment == Alignment.Left ? 'flex-start' : 'flex-end'

	return (
		<View style={{flex: 1, maxWidth: '70%', backgroundColor: color, borderRadius: 10, padding: 10, marginTop: 5, alignItems: align, alignSelf: align }}>
			<Text>{user}</Text>
			<Text>{children}</Text>
			<Text style={{ alignSelf: alignment == Alignment.Left ? 'flex-end' : 'flex-start'}}>{prettyPrintDateTime(new Date(time))}</Text>
		</View>
	)
});

export default Screen('textChannel', ({ params, nav }) => {
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
		connection.invoke('JoinChannel', params.channel.name)

		connection.on('messageReceived', (username: string, message: string, time: string) => {
			setNewMessage({
				userMail: username,
                body: message,
                dateTime: time
			})
		});
		
		return () => {
			connection.off('messageReceived')
			connection.invoke('LeaveChannel', params.channel.name)
		}

	}, []);

	const fetch = () => {
		return axios.get('/Channel/' + params.channel.id)
			.then(res => setMessages(res.data.messages));
	}

    const sendMessage = (msg: string) => {
		connection.invoke('NewMessage', email, msg, params.channel.name, params.channel.id)
            .catch(err => console.log(err));

		setMessage('');
	}

	return (
		<Loader load={fetch} style={{flex: 1}}>
			<FlatList ref={listRef} contentContainerStyle={{ flexGrow: 1, justifyContent: 'flex-end' }} data={messages} onContentSizeChange={() => listRef.current.scrollToEnd()} renderItem={
				({ item, index }) => ( //TODO: Change alignment & color based on userMail (like Messenger)
					<Message key={index} user={item.userMail} time={item.dateTime} color={item.userMail === email ? theme.primary : theme.accent} alignment={item.userMail === email ? Alignment.Right : Alignment.Left}>
						{item.body}
					</Message>
				)} />
				
			<TextInput value={message} placeholder={"send message in #" + params.channel.name} onChangeText={setMessage} onSubmitEditing={(e) => sendMessage(message)} returnKeyType="send"/>
		</Loader>
	)
})
