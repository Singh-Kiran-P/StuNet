import React, { screen } from '@/.';

import {
    View
} from 'react-native';

import {
    Button
} from 'react-native-paper';

export default screen('test4', ({ params, nav }) => {
    console.log(4);
    return (
        <View>
            <Button mode='contained' onPress={() => {
                nav.push('test5');
            }}>Go to Test 5</Button>
        </View>
    )
})
