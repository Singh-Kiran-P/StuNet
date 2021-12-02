import React, { Component, useTheme, paper } from '@/.';
import { Appbar } from 'react-native-paper';
import { replace } from '@/util/strings';

export default Component(({ params, params: { title }, nav }) => {
    let [theme] = useTheme();

    return (
        <Appbar.Header theme={paper(theme)}>
            {!nav.getState().index || <Appbar.BackAction onPress={() => nav.goBack()}/>}
            <Appbar.Content title={replace(title || '', params) || 'Loading...'}/>
        </Appbar.Header>
    )
})
