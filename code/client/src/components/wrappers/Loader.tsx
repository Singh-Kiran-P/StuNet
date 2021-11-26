import React, { Children, Theme, Style, useEffect, useState } from '@/.';
import { View, ActivityIndicator } from 'react-native';

type Props = Children & {
	load: () => Promise<any>
}

export default ({ load, children }: Props) => {
	const [loading, setLoading] = useState(true);

	useEffect(() => {
		load().then(() => setLoading(false)); // TODO catch error?
	}, [])

	if (!loading) return <View children={children}/>
	return <ActivityIndicator style={s.loading} size={Theme.huge} color={Theme.colors.primary}/>;
}

const s = Style.create({
	loading: {
		flex: 1,
		justifyContent: 'center'
	}
})
