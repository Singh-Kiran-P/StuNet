import React, { Screen, useTheme, extend } from "@/.";
import {  View, TextInput, Text, Loader } from '@/components'
import { useEffect, useRef, useState } from "react";
import { FlatList } from "react-native";
import { useConnection } from "@/connection";
import axios from "axios";
import { continueStatement } from "@babel/types";

enum Alignment { Left, Right };

type Message = {
	userMail: string,
	body: string,
	dateTime: string,
}

type Props = {
	user: string
	color: string,
	alignment: Alignment,
};

const Message = extend<typeof View, Props>(View, ({ user, color, alignment, children }) => {
	const align: string = alignment == Alignment.Left ? 'flex-start' : 'flex-end'

	return (
		<View style={{flex: 1, maxWidth: '70%', backgroundColor: color, borderRadius: 10, padding: 10, marginTop: 5, alignItems: align, alignSelf: align }}>
			<Text>{user}</Text>
			<Text style={{ textAlign: alignment == Alignment.Left ? 'left' : 'right'}}>{children}</Text>
		</View>
	)
});

export default Screen('textChannel', ({ params, nav }) => {
	const [message, setMessage] = useState('');
	const [messages, setMessages] = useState<Message[]>([]);
	const [newMessage, setNewMessage] = useState<Message>();
	const [username, setUsername] = useState<string>(new Date().getTime().toString());
	const connection = useConnection();

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
		connection.invoke('NewMessage', username, msg, params.channel.name, params.channel.id)
            .catch(err => console.log(err));

		setMessage('');
	}

	return (
		<Loader load={fetch} style={{flex: 1}}>
			<FlatList ref={listRef} contentContainerStyle={{ flexGrow: 1, justifyContent: 'flex-end' }} data={messages} onContentSizeChange={() => listRef.current.scrollToEnd()} renderItem={
				({ item, index }) => ( //TODO: Change alignment & color based on userMail (like Messenger)
					<Message key={index} user={item.userMail} color={theme.primary} alignment={Alignment.Right}>
						{item.body}
					</Message>
				)} />
				
			<TextInput value={message} placeholder={"send message in #" + params.channel.name} onChangeText={setMessage} onSubmitEditing={(e) => sendMessage(message)} returnKeyType="send"/>
		</Loader>
	)
})
