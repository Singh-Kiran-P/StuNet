import React, { createContext, useContext, useEffect, useState } from 'react';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Children } from '@/util';
import { useToken } from '@/auth';
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
		connection.start().catch(err => console.log(err)); // TODO handle error
		
		connection.on('QuestionNotification', (id: number) => console.log(id));
		connection.on('AnswerNotification', (id: number) => console.log(id));

		return () => {
			connection.stop().catch(err => console.log(err)) // TODO handle error
		}
	}, [])

	return <Context.Provider value={connection} children={children}/>
}
