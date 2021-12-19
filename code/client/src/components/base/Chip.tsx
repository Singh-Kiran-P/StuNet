import React, { extend, Size, theming } from '@/.';
import Text from '@/components/base/Text';

type Props = {
    size? : Size;
    active?: boolean;
}

export default extend<typeof Text, Props>(Text, ({ active, size, style, ...props }) => {
    const tag = theming(theme => ({
        color: active ? theme.bright : theme.foreground,
        backgroundColor: active ? theme.accent : theme.disabled,
        paddingVertical: theme.padding * 0.2,
        paddingHorizontal: theme.padding * 0.5
    }))

    return <Text
        radius='round'
        style={[tag, style]}
        size={size || 'small'}
        {...props}
    />
})
