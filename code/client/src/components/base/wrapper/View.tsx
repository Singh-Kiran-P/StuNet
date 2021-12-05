import React, { extend, Style } from '@/.';
import { View } from 'react-native';

type Props = {
    type?: keyof typeof s;
}

const s = Style.create({
    normal: {},

    header: {
        flexDirection: 'row',
        alignItems: 'center',
        flexWrap: 'wrap'
    }
})

export default extend<typeof View, Props>(View, ({ type, style, ...props }) => {
    return <View
        style={[s[type || 'normal'], style]}
        {...props}
    />
})
