import React, { extend, useTheme, paper } from '@/.';
import { TextInput } from 'react-native-paper';

export default extend(TextInput, props => {
   let [theme] = useTheme();

    return <TextInput
        mode='outlined'
        numberOfLines={props.multiline ? 5 : undefined}
        theme={paper(theme)}
        {...props}
    />
})
