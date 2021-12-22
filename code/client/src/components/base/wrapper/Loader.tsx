import React, { extend, useEffect, useState, useNav, useParams } from '@/.';
import View from '@/components/base/wrapper/View';
import Spinner from '@/components/base/Spinner';

type Props = {
	load?: () => Promise<any>;
	state?: boolean;
}

export default extend<typeof View, Props>(View, ({ load, state, ...props }) => {
	let { update } = (load && useParams()) || { update: 0 };
	let [loading, setLoading] = useState(!!load);
	let nav = useNav();

	if (load) useEffect(() => {
		load().then(() => {
			setLoading(false);
		}).catch(err => {
			if (nav?.canGoBack()) nav.goBack();
			// TODO system message
		})
	}, [update])

	if (state === true || loading) return <Spinner/>
	return <View flex {...props}/>
})
