import React, { Component, Children, theming } from '@/.';

import {
    View,
    ScrollView
} from 'react-native';

export default Component<Children>(({ children, params: { scroll = true, padding = true } }) => {
    const s = theming(theme => {
        if (padding === true) padding = theme.padding;
        return {
            flex: 1,
            padding: padding || undefined,
            backgroundColor: theme.background
        }
    })

    if (!scroll) return <View style={s} children={children}/>
    return <ScrollView contentContainerStyle={s} children={children}/>
})

// TODO SCROLL
