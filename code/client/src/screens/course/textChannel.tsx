import React, { Screen, useTheme, extend } from "@/.";
import { ScrollView, View, TextInput, Text } from '@/components'
import { useEffect, useState } from "react";
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
	const margin = alignment == Alignment.Left ? {marginRight: 'auto'} : {marginLeft: 'auto'}

	return (
		<View style={{maxWidth: '70%', backgroundColor: color, borderRadius: 10, padding: 10, marginTop: 5, ...margin }}>
			<Text style={margin}>{user}</Text>
			<Text>{children}</Text>
		</View>
	)
})

export default Screen('textChannel', ({ params, nav }) => {
	const [message, setMessage] = useState('');
    const [messages, setMessages] = useState<Message[]>([]);

	const [connection, setConnection] = useState<signalR.HubConnection>();
    const username = new Date().getTime();
    let [theme] = useTheme();

    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("http://10.0.2.2:5000/chat")
            .build();
        setConnection(connection)

        connection.on("messageReceived", (username: string, message: string) => {
            let m = {
                sender: username,
                content: message,
                time: '0'
            }
            setMessages([...messages, m])
        });

        connection
            .start()
            .catch(err => console.log(err));

    }, []);

    const sendMessage = (msg: string) => {

        connection!.send("newMessage", username, msg)
            .catch(err => console.log(err));


		setMessage('');

	}

	return (
		<View flex>
			<ScrollView style={{flexGrow: 1, flexDirection: 'column-reverse' }}>
				{messages.map((msg, i) => ( //TODO: Change alignment & color based on sender (like Messenger)
					<Message key={i} user={msg.sender} color={theme.primary} alignment={Alignment.Right}>
						{msg.content}
					</Message>
				))}
			</ScrollView>
			<TextInput value={message} placeholder={"send message in #" + params.name} onChangeText={setMessage} onSubmitEditing={(e) => sendMessage(message)} returnKeyType="send"/>
		</View>
	)
})
