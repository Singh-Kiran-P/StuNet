import React, { extend, Style, Theme } from '@/.';
import { Text } from 'react-native-paper';

type Props = {
    mode?: keyof typeof s;
}

export default extend<typeof Text, Props>(Text, ({ mode, style, ...props }) => {
    return <Text
        {...props}
        style={[s[mode || 'normal'], style]}
    />
})

const s = Style.create({

    normal: {

    },

    header: {
        fontWeight: 'bold',
        fontSize: Theme.large
    }

})
