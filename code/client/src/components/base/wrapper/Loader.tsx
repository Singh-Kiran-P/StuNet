import React, { Style, extend, useTheme, useEffect, useState, useNav } from '@/.';
import View from '@/components/base/wrapper/View';
import { ActivityIndicator } from 'react-native';

type Props = {
	load?: () => Promise<any>;
	state?: boolean;
}

export default extend<typeof View, Props>(View, ({ load, state, style, ...props }) => {
	let [loading, setLoading] = useState(!!load);
	let [theme] = useTheme();
	let nav = useNav();
	const s = Style.create({
		loading: {
			flex: 1,
			justifyContent: 'center',
			backgroundColor: theme.background
		},

		loaded: {
			flex: 1
		}
	})

	if (load) useEffect(() => {
		load().then(() => {
			setTimeout(() => setLoading(false));
		}).catch(err => {
			if (nav?.canGoBack()) nav.goBack();
			// TODO system message
		})
	}, [])

	if (state !== true && !loading) return <View style={[s.loaded, style]} {...props}/>
	return <ActivityIndicator style={s.loading} size={theme.huge} color={theme.primary}/>
})
