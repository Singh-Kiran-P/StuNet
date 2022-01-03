import React, { extend, useTheme, Style, Size, Color } from '@/.';
import { ActivityIndicator } from 'react-native';

type Props = {
    size: never;
    color: never;
    sizing?: Size;
    coloring?: Color;
    background?: boolean;
}

export default extend<typeof ActivityIndicator, Props>(ActivityIndicator, ({ style, size, color, sizing, coloring, background, ...props }) => {
    let [theme] = useTheme();

    const s = Style.create({
		spinner: {
            flex: 1,
			justifyContent: 'center'
		},

        background: !background ? {} : {
            backgroundColor: theme.background
        }
	})

    return <ActivityIndicator
        style={[s.spinner, s.background, style]}
        color={theme[coloring || 'accent']}
        size={theme[sizing || 'massive']}
        {...props}
    />
})
