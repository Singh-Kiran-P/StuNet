import React, { extend, useTheme, paper, Style, Color } from '@/.';
import { FAB } from 'react-native-paper';

type Props = {
    background?: Color | false;
}

export default extend<typeof FAB, Props>(FAB, ({ background, style, color, ...props }) => {
    let [theme] = useTheme();

    const s = Style.create({
        fab: {
            backgroundColor: theme[background || 'accent'],
            position: 'absolute',
            bottom: 0,
            right: 0
        }
    })

    return <FAB
        theme={paper(theme)}
        style={[style, s.fab]}
        color={color || theme.bright}
        {...props}
    />
})
