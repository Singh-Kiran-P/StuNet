import { Appbar } from 'react-native-paper';
import { replace } from '@/util/strings';
import React, { Component } from '@/.';

export default Component(({ params, params: { title }, nav }) => {
    return (
        <Appbar.Header>
            {!nav.getState().index || <Appbar.BackAction onPress={() => nav.goBack()}/>}
            <Appbar.Content title={replace(title, params)}/>
        </Appbar.Header>
    )
})
