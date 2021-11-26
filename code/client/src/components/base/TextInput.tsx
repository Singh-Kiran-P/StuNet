import { TextInput } from 'react-native-paper';
import React, { Props } from '@/.';

export default Object.assign((props: Partial<Props<typeof TextInput>>) => {
    return <TextInput
        mode='outlined'
        numberOfLines={props.multiline ? 5 : undefined}
        {...props as Props<typeof TextInput>}
    />
}, TextInput)
