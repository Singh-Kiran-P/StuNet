import React, { extend, useTheme, paper } from '@/.';
import { Searchbar } from 'react-native-paper';

export default extend(Searchbar, props => {
    let [theme] = useTheme();

    return <Searchbar
        theme={paper(theme)}
        {...props}
    />
})
