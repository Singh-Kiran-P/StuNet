import React, { extend, useTheme, paper } from '@/.';
import { Checkbox } from 'react-native-paper';

export default extend(Checkbox.Item, props => {
    let [theme] = useTheme();

    return <Checkbox.Item
        mode='ios'
        theme={paper(theme)}
        {...props}
    />
})
