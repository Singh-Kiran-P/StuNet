import React, { Screen, useTheme, extend } from "@/.";
import { ScrollView, View, TextInput, Text } from '@/components'
import { useState } from "react";

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
	let [theme] = useTheme();

	const sendMessage = (msg: string) => {
		console.log(msg);
		setMessage('');
		let m = {
			sender: 'testUser',
			content: msg,
			time: '0'
		}
		setMessages([...messages, m])
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