import React, { component } from '@/.';

import {
    Appbar
} from 'react-native-paper';

export default component(({ params: { title }, nav }) => {
    return (
        <Appbar.Header>
            {!nav.getState().index || <Appbar.BackAction onPress={() => nav.goBack()}/>}
            <Appbar.Content title={title}/>
        </Appbar.Header>
    )
})
