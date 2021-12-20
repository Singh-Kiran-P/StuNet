import React, { createContext, useContext, useEffect } from "react";
import * as signalR from "@microsoft/signalr";
import { Children } from '@/util';

const Context = createContext<signalR.HubConnection>(new signalR.HubConnectionBuilder().withUrl("http://10.0.2.2:5000/chat").build())
export const useConnection = () => useContext(Context)

export default ({ children }: Children) => {
	const connection = useConnection();

	useEffect(() => {
		connection.start()
			.catch(err => console.log(err));

		return () => {
			connection.stop()
				.catch(err => console.log(err))
		}
	}, [])

	return (
		< Context.Provider value={connection} >
			{ children }
		</Context.Provider >
	)
}

// TODO ????????????????????????????????
