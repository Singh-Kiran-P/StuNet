import React, { extend, Style, Theme } from '@/.';
import { Text } from 'react-native-paper';

type Props = {
    type?: keyof typeof s;
    visible?: boolean;
}

export default extend<typeof Text, Props>(Text, ({ type, style, visible = true, ...props }) => {
    return !visible ? null : <Text {...props}
        style={[s[type || 'normal'], style]}
    />
})

const s = Style.create({

    normal: {

    },

    header: {
        fontWeight: 'bold',
        fontSize: Theme.large
    },

    error: {
        color: 'red'
    }

})
