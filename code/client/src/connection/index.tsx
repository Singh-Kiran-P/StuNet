import React, { createContext, useContext, useEffect, useState } from 'react';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import notifee, { AndroidImportance, AndroidStyle } from '@notifee/react-native';
import { Children, Question, Answer } from '@/util';
import { useToken } from '@/auth';
import axios from 'axios';
import { update } from '@/.';

const Context = createContext<HubConnection>(null as any as HubConnection);
export const useConnection = () => useContext(Context);

export default ({ children }: Children) => {
	let [token] = useToken();
    let [connection] = useState(() => {
		return new HubConnectionBuilder().withUrl(axios.defaults.baseURL + '/chat', {
			accessTokenFactory: () => token
		}).build();
	})

	useEffect(() => {
		connection.start() // TODO handle error
			.catch(err => console.log(err));
		
		connection.on('QuestionNotification', async (question: Question) => {
			const channelId = await notifee.createChannel({
				id: 'Question',
				name: 'Course notifications',
				importance: AndroidImportance.HIGH,
			});
		
			await notifee.displayNotification({
				title: 'Question asked in ' + question.course.name,
				body: question.title,
				android: {
					channelId,
					smallIcon: 'ic_launcher',
					style: { type: AndroidStyle.BIGTEXT, text: question.body }
				},
			});
			update('Notifications');
		});

		connection.on('AnswerNotification', async (answer: Answer) => {
			const channelId = await notifee.createChannel({
				id: 'Answer',
				name: 'Question notifications',
				importance: AndroidImportance.HIGH,
			});
		
			await notifee.displayNotification({
				title: 'Answer received',
				body: answer.question.title,
				android: {
					channelId,
					smallIcon: 'ic_launcher',
					style: { type: AndroidStyle.BIGTEXT, text: answer.body }
				},
			});
			update('Notifications');
		});

		return () => {
			connection.off('QuestionNotification')
			connection.off('AnswerNotification')
			connection.stop() // TODO handle error
				.catch(err => console.log(err))
		}
	}, [])

	return <Context.Provider value={connection} children={children}/>
}
