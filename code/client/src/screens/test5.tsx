import React, { screen } from '@/.';

import {
    View
} from 'react-native';

import {
    Button
} from 'react-native-paper';

export default screen('test5', ({ params, nav }) => {
    console.log(5);
    return (
        <View>
            <Button mode='contained' onPress={() => {
                nav.push('test1', { name: 'bruh' });
            }}>Go to Test 1</Button>
        </View>
    )
})
