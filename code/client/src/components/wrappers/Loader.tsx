import React, { Children, Theme, Style, useEffect, useState, useNav } from '@/.';
import { View, ActivityIndicator } from 'react-native';

type Props = Children & {
	load?: () => Promise<any>;
	state?: boolean;
}

export default ({ load, state, children }: Props) => {
	const [loading, setLoading] = useState(!!load);
	let nav = useNav();

	if (load) useEffect(() => {
		load().then(() => setLoading(false)).catch(err => {
			if (nav?.canGoBack()) nav.goBack();
			// TODO system message
		})
	}, [])

	if (state !== true && !loading) return <View style={s.loaded} children={children}/>
	return <ActivityIndicator style={s.loading} size={Theme.huge} color={Theme.colors.home.primary}/>
}

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
