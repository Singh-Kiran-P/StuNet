import React, { createContext, useContext, useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { Children } from '@/util';
import { useToken } from "@/auth";

const Context = createContext<signalR.HubConnection>()
export const useConnection = () => useContext(Context)

export default ({ children }: Children) => {
	const token = useToken()[0];
	const connection = useState(new signalR.HubConnectionBuilder().withUrl("http://10.0.2.2:5000/chat").build())[0];

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