import notifee, { AndroidImportance, AndroidStyle } from '@notifee/react-native';
import React, { createContext, useContext, useEffect, useState } from 'react';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Children, Question, Answer } from '@/util';
import { useToken } from '@/auth';
import { update } from '@/nav';
import axios from 'axios';

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
		connection.start();

		connection.on('QuestionNotification', (question: Question) => {
			update('Notifications');
			notifee.createChannel({
				importance: AndroidImportance.HIGH,
				name: 'Course notifications',
				id: 'Question'
			}).then(channelId => {
				notifee.displayNotification({
					title: `Question asked in ${question.course.name}`,
					body: question.title,
					android: {
						channelId,
						smallIcon: 'ic_launcher',
						style: {
							type: AndroidStyle.BIGTEXT,
							text: question.title
						}
					}
				})
			})
		})

		connection.on('AnswerNotification', (answer: Answer) => {
			update('Notifications');
			notifee.createChannel({
				importance: AndroidImportance.HIGH,
				name: 'Question notifications',
				id: 'Answer'
			}).then(channelId => {
				notifee.displayNotification({
					title: 'Answer received',
					body: answer.title,
					android: {
						channelId,
						smallIcon: 'ic_launcher',
						style: {
							type: AndroidStyle.BIGTEXT,
							text: answer.title
						}
					}
				})
			})
		})

		return () => {
			connection.off('QuestionNotification');
			connection.off('AnswerNotification');
			connection.stop();
		}
	}, [])

	return <Context.Provider value={connection} children={children}/>
}
