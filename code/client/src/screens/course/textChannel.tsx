import React, { Screen, useTheme, extend } from "@/.";
import {  View, TextInput, Text } from '@/components'
import { useEffect, useRef, useState } from "react";
import { FlatList } from "react-native";
import { useConnection } from "@/connection";

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
	const [username, setUsername] = useState<number>(new Date().getTime());
	const connection = useConnection();

	let [theme] = useTheme();
	let listRef = useRef();
	
	useEffect(() => {
		if (newMessage) {
			setMessages([...messages, newMessage])
		}
	}, [newMessage])

	useEffect(() => {
		connection.invoke('JoinChannel', params.name)

		connection.on("messageReceived", (username: string, message: string) => {
			setNewMessage({
				sender: username,
                content: message,
                time: '0'
			})
		});
		
		return () => {
			connection.off("messageReceived")
			connection.invoke('LeaveChannel', params.name)
		}

	}, []);

    const sendMessage = (msg: string) => {
        connection.send("newMessage", username, msg, params.name)
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
