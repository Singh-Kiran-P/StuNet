import React, { extend } from '@/.';
import { TextInput } from 'react-native-paper';

export default extend(TextInput, props => {
    return <TextInput
        mode='outlined'
        numberOfLines={props.multiline ? 5 : undefined}
        {...props}
    />
})
