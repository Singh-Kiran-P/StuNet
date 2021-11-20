import React, { component, Children, Theme } from '@/.';

import {
    View,
    ScrollView,
    StyleSheet
} from 'react-native';

export default component<Children>(({ children, params: { scroll, padding } }) => {
    const Container = scroll ? ScrollView : View;
    if (padding === true) padding = Theme.padding;
    const style = StyleSheet.create({
        screen: {
            padding: padding || undefined
        }
    })

    return (
        <Container style={style.screen}>
            {children}
        </Container>
    )
})
