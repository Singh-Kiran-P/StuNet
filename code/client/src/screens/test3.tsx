import React, { screen } from '@/.';

import {
    View
} from 'react-native';

import {
    Button
} from 'react-native-paper';

export default screen('test3', ({ params, nav }) => {
    console.log(3);
    return (
        <View>
            <Button mode='contained' onPress={() => {
                nav.push('test4');
            }}>Go to Test 4</Button>
        </View>
    )
})
