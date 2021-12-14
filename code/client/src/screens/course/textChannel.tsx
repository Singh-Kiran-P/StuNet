import React, { Screen, useTheme, extend } from "@/.";
import {  View, TextInput, Text } from '@/components'
import { useEffect, useRef, useState } from "react";
import { FlatList } from "react-native";
import * as signalR from "@microsoft/signalr";

// https://github.com/dotnet/aspnetcore/issues/38286#issuecomment-970580861
if (!globalThis.document) {
	(globalThis.document as any) = undefined;
}

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
	const [connection, setConnection] = useState<signalR.HubConnection>(new signalR.HubConnectionBuilder().withUrl("http://10.0.2.2:5000/chat").build());
	const [username, setUsername] = useState<number>(new Date().getTime());

	let [theme] = useTheme();
	let listRef = useRef();
	
	useEffect(() => {
		if (newMessage) {
			setMessages([...messages, newMessage])
		}
	}, [newMessage])

    useEffect(() => {
		connection.on("messageReceived", (username: string, message: string) => {
			setNewMessage({
				sender: username,
                content: message,
                time: '0'
			})
        });

        connection.start()
			.catch(err => console.log(err));

        return () => {
            connection.stop()
                .catch(err => console.log(err))

        }
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
