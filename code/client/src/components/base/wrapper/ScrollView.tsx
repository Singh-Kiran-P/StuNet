import React, { extend } from '@/.';
import { ScrollView } from 'react-native';

export default extend(ScrollView, props => {
    return <ScrollView
        {...props}
    />
})
