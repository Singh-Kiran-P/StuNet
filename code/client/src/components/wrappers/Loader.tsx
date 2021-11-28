import React, { Children, Style, useTheme, useEffect, useState, useNav } from '@/.';
import { View, ActivityIndicator } from 'react-native';

type Props = Children & {
	load?: () => Promise<any>;
	state?: boolean;
}

export default ({ load, state, children }: Props) => {
	let [loading, setLoading] = useState(!!load);
	let [theme] = useTheme();
	let nav = useNav();

	const s = Style.create({

		loading: {
			flex: 1,
			justifyContent: 'center'
		},

		loaded: {
			width: '100%',
			height: '100%'
		}

	})

	if (load) useEffect(() => {
		load().then(() => setLoading(false)).catch(err => {
			if (nav?.canGoBack()) nav.goBack();
			// TODO system message
		})
	}, [])

	if (state !== true && !loading) return <View style={s.loaded} children={children}/>
	return <ActivityIndicator style={s.loading} size={theme.huge} color={theme.primary}/>
}
