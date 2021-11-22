import React, { Component, Children, Theme, Style } from '@/.';

import {
    View,
    ScrollView
} from 'react-native';

export default Component<Children>(({ children, params: { scroll = true, padding = true } }) => {
    if (padding === true) padding = Theme.padding;
    let pad = { padding: padding || undefined };

    if (!scroll) return <View style={[style.screen, pad]} children={children}/>
    return <ScrollView style={style.screen} contentContainerStyle={pad} children={children}/>
})

const style = Style.create({
    screen: {
        width: '100%',
        height: '100%',
        backgroundColor: Theme.colors.background,
        color: Theme.colors.onSurface,
    }
})
