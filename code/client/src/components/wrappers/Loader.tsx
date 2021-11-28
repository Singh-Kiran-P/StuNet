import React, { Style, useTheme, useEffect, useState, useNav } from '@/.';
import { View, ActivityIndicator, ViewProps } from 'react-native';

type Props = ViewProps & {
	load?: () => Promise<any>;
	state?: boolean;
}

export default ({ load, state, children, style }: Props) => {
	let [loading, setLoading] = useState(!!load);
	let [theme] = useTheme();
	let nav = useNav();

	if (load) useEffect(() => {
		load().then(() => setLoading(false)).catch(err => {
			if (nav?.canGoBack()) nav.goBack();
			// TODO system message
		})
	}, [])

	return <View style={[style, {flex: 1}, (loading || state) ? {justifyContent: 'center'} : null]}>
		{(state !== true && !loading) ? children : <ActivityIndicator size={theme.huge} color={theme.primary} />}
	</View>
}
