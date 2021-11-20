import React, { component } from '@/.';

import {
    Appbar
} from 'react-native-paper';

export default component(({ params, params: { title }, nav }) => {
    return (
        <Appbar.Header>
            {nav.canGoBack() && <Appbar.BackAction onPress={() => nav.goBack()}/>}
            <Appbar.Content title={typeof title === 'function' ? title(params) : title}/>
        </Appbar.Header>
    )
})
