import React, { extend, animate } from '@/.';
import { List } from 'react-native-paper';

export default extend(List.Accordion, props => {
    return <List.Accordion
        onPress={animate}
        {...props}
    />
})
