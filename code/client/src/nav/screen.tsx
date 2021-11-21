import React, { Component, Children, Theme, Style } from '@/.';

import {
    View,
    ScrollView
} from 'react-native';

export default Component<Children>(({ children, params: { scroll = true, padding = true } }) => {
    if (padding === true) padding = Theme.padding;
    const Container = scroll ? ScrollView : View;
    const style = Style.create({
        screen: {
            width: '100%',
            height: '100%',
            padding: padding || undefined,
            backgroundColor: Theme.colors.background,
            color: Theme.colors.onSurface,
        }
    })

    return (
        <Container style={style.screen}>
            {children}
        </Container>
    )
})
