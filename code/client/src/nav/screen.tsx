import React, { Component, Children, theming } from '@/.';
import { View, ScrollView } from '@/components';

export default Component<Children>(({ children, params: { scroll = true, padding = true } }) => {
    const s = theming(theme => {
        if (padding === true) padding = theme.padding;
        return {
            padding: padding || undefined,
            backgroundColor: theme.background,
            [scroll ? 'minHeight' : 'height']: '100%',
            width: '100%'
        }
    })

    if (!scroll) return <View style={s} children={children}/>
    return <ScrollView contentContainerStyle={s} children={children}/>
})
