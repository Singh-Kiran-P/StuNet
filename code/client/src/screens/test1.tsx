import React, { screen } from '@/.';

import {
    View
} from 'react-native';

import {
    Button
} from 'react-native-paper';

export default screen('test1', ({ params, nav }) => {
    console.log(1);
    return (
        <View>
            <Button mode='contained' onPress={() => {
                nav.push('test2');
            }}>Go to Test 2</Button>
        </View>
    )
})
