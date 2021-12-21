import React, { extend, useTheme, Style, Size } from '@/.';
import { Text } from 'react-native-paper';

type Props = {
    type?: 'normal' | 'header' | 'title' | 'error' | 'hint' | 'link';
    size?: 'auto' | Size;
}

export default extend<typeof Text, Props>(Text, ({ type, size, style, ...props }) => {
    let [theme] = useTheme();

    const fontSize = (s: Size) => ({
        fontSize: size === undefined ? theme[s] : size === 'auto' ? undefined : theme[size]
    })

    const s = Style.create({
        normal: {
            color: theme.foreground,
            ...fontSize('normal')
        },

        header: {
            color: theme.foreground,
            ...fontSize('large'),
            fontWeight: 'bold'
        },

        title: {
            marginBottom: theme.padding,
            color: theme.primary,
            ...fontSize('large'),
            fontWeight: 'bold'
        },

        error: {
            color: theme.error,
            ...fontSize('normal')
        },

        hint: {
            color: theme.placeholder,
            ...fontSize('small')
        },

        link: {
            color: theme.accent,
            ...fontSize('normal'),
            textDecorationLine: 'underline'
        }
    })

    return <Text
        style={[s[type || 'normal'], style]}
        {...props}
    />
})
