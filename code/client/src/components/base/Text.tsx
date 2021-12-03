import React, { extend, useTheme, Style } from '@/.';
import { Text } from 'react-native-paper';

type Props = {
    type?: 'normal' | 'header' | 'error' | 'hint' | 'link';
    size?: '' | 'small' | 'medium' | 'large' | 'huge';
    visible?: boolean;
}

export default extend<typeof Text, Props>(Text, ({ type, size, style, visible = true, ...props }) => {
    let [theme] = useTheme();

    const fontSize = (s: NonNullable<Exclude<Props['size'], ''>>) => ({
        fontSize: size === undefined ? theme[s] : size ? theme[size] : undefined
    })

    const s = Style.create({

        normal: {
            color: theme.foreground,
            ...fontSize('medium')
            
        },

        header: {
            color: theme.foreground,
            ...fontSize('large'),
            fontWeight: 'bold'
        },

        error: {
            color: theme.error,
            ...fontSize('medium')
        },

        hint: {
            color: theme.placeholder,
            ...fontSize('small')
        },

        link: {
            color: theme.accent,
            ...fontSize('medium'),
            textDecorationLine: 'underline'
        }

    })

    if (!visible) return null;
    return <Text
        style={[s[type || 'normal'], style]}
        {...props}
    />
})
