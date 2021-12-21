import React, { extend, useTheme, Style, Size, Color } from '@/.';
import { ActivityIndicator } from 'react-native';

type Props = {
    size: never;
    color: never;
    sizing?: Size;
    coloring?: Color;
}

export default extend<typeof ActivityIndicator, Props>(ActivityIndicator, ({ style, size, color, sizing, coloring, ...props }) => {
    let [theme] = useTheme();

    const s = Style.create({
		spinner: {
            flex: 1,
			justifyContent: 'center'
		}
	})

    return <ActivityIndicator
        style={[s.spinner, style]}
        size={theme[sizing || 'massive']}
        color={theme[coloring || 'accent']}
        {...props}
    />
})
