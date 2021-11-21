import React, { screen } from '@/.';

import {
    View
} from 'react-native';

import {
    Button
} from 'react-native-paper';

export default screen('test2', ({ params, nav }) => {
    console.log(2);
    return (
        <View>
            <Button mode='contained' onPress={() => {
                nav.push('test3');
            }}>Go to Test 3</Button>
        </View>
    )
})
