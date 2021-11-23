import React, { useEffect, useState } from '@/.';
import { View } from 'react-native';
import { ActivityIndicator } from 'react-native-paper';

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
		loading ? <ActivityIndicator size='large' animating={loading} hidesWhenStopped/> :
			<View>
				{props.children}
			</View>
	)
}