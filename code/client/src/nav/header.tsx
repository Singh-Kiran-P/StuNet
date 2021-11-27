import { Appbar, useTheme } from 'react-native-paper';
import { replace } from '@/util/strings';
import React, { Component } from '@/.';

export default Component(({ params, params: { title }, nav }) => {
    const theme = useTheme();

    return (
        <Appbar.Header style={{backgroundColor: theme.colors.primary}}>
            {!nav.getState().index || <Appbar.BackAction onPress={() => nav.goBack()}/>}
            <Appbar.Content title={replace(title, params)}/>
        </Appbar.Header>
    )
})
