import React, { extend, useTheme } from '@/.';
import { Text } from 'react-native-paper';

type Props = {
    type?: 'normal' | 'header' | 'error' | 'hint' | 'link';
    size?: '' | 'small' | 'medium' | 'large' | 'huge';
    visible?: boolean;
}

export default extend<typeof Text, Props>(Text, ({ type, size, style, visible = true, ...props }) => {
    let [theme] = useTheme();

    const s = {

        normal: {
            color: theme.foreground,
            fontSize: theme.medium,

        },

        header: {
            color: theme.foreground,
            fontSize: theme.large,
            fontWeight: 'bold'
        },

        error: {
            color: theme.error,
            fontSize: theme.medium
        },

        hint: {
            color: theme.foreground,
            fontSize: theme.small
        },

        link: {
            color: theme.accent,
            fontSize: theme.medium,
            textDecorationLine: 'underline'
        }

    }[type || 'normal'] as any;

    if (size !== undefined) s.fontSize = size ? theme[size] : undefined;

    if (!visible) return null;
    return <Text
        style={[s, style]}
        {...props}
    />
})
