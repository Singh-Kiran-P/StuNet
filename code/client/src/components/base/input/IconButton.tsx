import React, { extend, useTheme, paper } from '@/.';
import { IconButton } from 'react-native-paper';

export default extend(IconButton, props => {
    let [theme] = useTheme();

    return <IconButton
        theme={paper(theme)}
        {...props}
    />
})
