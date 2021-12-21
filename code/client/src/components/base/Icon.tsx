import React, { extend, useTheme, Size, Color } from '@/.';
import Icon from 'react-native-vector-icons/MaterialCommunityIcons';

type Props = {
    size: never;
    color: never;
    sizing?: Size;
    coloring?: Color;
}

export default extend<typeof Icon, Props>(Icon, ({ size, color, sizing, coloring, ...props }) => {
    let [theme] = useTheme();

    return <Icon
        size={sizing && theme[sizing]}
        color={theme[coloring || 'placeholder']}
        {...props}
    />
})
