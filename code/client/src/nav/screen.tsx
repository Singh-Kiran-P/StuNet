import React, { Component, Children, theming } from '@/.';
import { View, ScrollView } from '@/components';

export default Component<Children>(({ children, params: { scroll = true, padding, paddingTop, paddingBottom } }) => {
    const s = theming(theme => {
        const set = (p?: number | boolean) => p === false ? 0 : p === true ? theme.padding : p;
        if (padding === undefined) padding = true;
        paddingBottom = set(paddingBottom);
        paddingTop = set(paddingTop);
        padding = set(padding);
        return {
            padding: padding,
            paddingTop: paddingTop,
            paddingBottom: paddingBottom,
            backgroundColor: theme.background,
            [scroll ? 'minHeight' : 'height']: '100%',
            width: '100%'
        }
    })

    if (!scroll) return <View style={s} children={children}/>
    return <ScrollView contentContainerStyle={s} children={children}/>
})
