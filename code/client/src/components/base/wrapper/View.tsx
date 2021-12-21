import React, { extend, Style } from '@/.';
import { View } from 'react-native';

export type Props = {
    type?: keyof typeof s;
}

const s = Style.create({
    normal: {},

    header: {
        flexDirection: 'row',
        alignItems: 'center',
        flexWrap: 'wrap'
    },

    row: {
        flexDirection: 'row'
    }
})

export default extend<typeof View, Props>(View, ({ type, style, ...props }) => {
    return <View
        style={[s[type || 'normal'], style]}
        {...props}
    />
})
