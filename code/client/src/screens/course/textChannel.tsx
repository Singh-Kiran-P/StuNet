import React, { Screen, useTheme, extend } from "@/.";
import {  View, TextInput, Text } from '@/components'
import { useEffect, useRef, useState } from "react";
import { FlatList } from "react-native";
import * as signalR from "@microsoft/signalr";

enum Alignment { Left, Right };

type Message = {
	sender: string,
	content: string,
	time: string,
}

type Props = {
	user: string
	color: string,
	alignment: Alignment
};

const Message = extend<typeof View, Props>(View, ({ user, color, alignment, children }) => {
	const margin = alignment == Alignment.Left ? { marginRight: 'auto' } : { marginLeft: 'auto' }

	return (
		<View style={{ maxWidth: '70%', backgroundColor: color, borderRadius: 10, padding: 10, marginTop: 5, ...margin }}>
			<Text style={margin}>{user}</Text>
			<Text>{children}</Text>
		</View>
	)
});

export default Screen('textChannel', ({ params, nav }) => {
	const [message, setMessage] = useState('');
	const [messages, setMessages] = useState<Message[]>([]);
	const [newMessage, setNewMessage] = useState<Message>();

	const [connection, setConnection] = useState<signalR.HubConnection>();
    const username = new Date().getTime();
	let [theme] = useTheme();
	let listRef = useRef();
	
	useEffect(() => {
		if (newMessage) {
			setMessages([...messages, newMessage])
		}
	}, [newMessage])

    useEffect(() => {
        const conn = new signalR.HubConnectionBuilder()
            .withUrl("http://10.0.2.2:5000/chat")
            .build();
        setConnection(conn)

		conn.on("messageReceived", (username: string, message: string) => {
			setNewMessage({
				sender: username,
                content: message,
                time: '0'
			})
        });

        conn
            .start()
			.catch(err => console.log(err));

	}, []);

    const sendMessage = (msg: string) => {
        connection!.send("newMessage", username, msg)
            .catch(err => console.log(err));

		setMessage('');
	}

	return (
		<View style={{flex: 1}}>
			<FlatList ref={listRef} contentContainerStyle={{ flexGrow: 1, justifyContent: 'flex-end' }} data={messages} onContentSizeChange={() => listRef.current.scrollToEnd()} renderItem={
					({item, index, separators }) => ( //TODO: Change alignment & color based on sender (like Messenger)
					<Message key={index} user={item.sender} color={theme.primary} alignment={Alignment.Right}>
						{item.content}
					</Message>
				)} />
				
			<TextInput value={message} placeholder={"send message in #" + params.name} onChangeText={setMessage} onSubmitEditing={(e) => sendMessage(message)} returnKeyType="send"/>
		</View>
	)
})
