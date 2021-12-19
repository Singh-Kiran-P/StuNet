import React, { extend, useTheme, paper } from '@/.';
import { TouchableRipple } from 'react-native-paper';
import View, { Props } from '@/components/base/wrapper/View';

export default extend<typeof TouchableRipple, Props>(TouchableRipple, ({
    borderless,
    background,
    centered,
    disabled,
    onPress,
    onLongPress,
    rippleColor,
    underlayColor,
    theme,
    style,
    ...props
}) => {
    let [theming] = useTheme();

    return <TouchableRipple
        borderless={borderless}
        background={background}
        centered={centered}
        disabled={disabled}
        onPress={onPress}
        onLongPress={onLongPress}
        rippleColor={rippleColor}
        underlayColor={underlayColor}
        theme={theme || paper(theming)}
        style={style}
    >
        <View {...props}/>
    </TouchableRipple>
})
