import React, { useEffect, useState } from '@/.';
import { Theme } from '@/css';
import { View, ActivityIndicator, Platform } from 'react-native';
// import { ActivityIndicator } from 'react-native-paper';

type Props = {
	func: () => Promise<any>,
	children?: JSX.Element[]
}

export default (props: Props) => {
	const [loading, setLoading] = useState(true);
	
	useEffect(() => {
		props.func().then(() => {
			setLoading(false);
		});
	}, [])

	return (
		loading?
			<ActivityIndicator style={{ flex: 1, justifyContent: 'center' }} size={Theme.huge} color={Theme.colors.primary}/> :
			<View>
				{props.children}
			</View>
	)
}