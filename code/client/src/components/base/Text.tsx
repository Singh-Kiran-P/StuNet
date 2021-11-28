import React, { extend, Style, useTheme } from '@/.';
import { Text } from 'react-native-paper';

type Props = {
    type?: 'normal' | 'header' | 'error' | 'hint';
    visible?: boolean;
}

export default extend<typeof Text, Props>(Text, ({ type, style, visible = true, ...props }) => {
    let [theme] = useTheme();

    const s = Style.create({

        normal: {
            color: theme.foreground,
            fontSize: theme.medium
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
        }
    
    })

    return !visible ? null : <Text {...props}
        style={[s[type || 'normal'], style]}
    />
})
