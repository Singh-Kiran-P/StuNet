import React, { screen } from '@/.';

import {
    View,
    Text,
    Button,
    TextInput
} from 'react-native';

export default screen('test5', ({ params, nav }) => {
    console.log('5');
    return (
        <View>
            <Text>5: {JSON.stringify(params)}</Text>
            <Button title='nav 1' onPress={() => {
                nav.push('test1', { name: '5' });
            }}/>
            <Button title='nav 2' onPress={() => {
                nav.push('test2');
            }}/>
            <Button title='nav 3' onPress={() => {
                nav.push('test3', { param3: 5 });
            }}/>
            <Button title='nav 4' onPress={() => {
                nav.push('test4');
            }}/>
            <Button title='nav 5' onPress={() => {
                nav.push('test5');
            }}/>
            <TextInput placeholder='test 5'/>
        </View>
    )
})
