import React, { Component, Children, theming } from '@/.';
import { View, ScrollView } from '@/components';

export default Component<Children>(({ children, params: { scroll = true, padding } }) => {
    const s = theming(theme => {
        if (padding === undefined) padding = true;
        if (padding === true) padding = theme.padding;

        return {
            padding: padding || 0,
            backgroundColor: theme.background,
            [scroll ? 'minHeight' : 'height']: '100%',
            width: '100%'
        }
    })

    if (!scroll) return <View style={s} children={children}/>
    return <ScrollView contentContainerStyle={s} children={children}/>
})
