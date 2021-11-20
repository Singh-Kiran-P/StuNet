import React, { screen } from '@/.';

import {
    View,
    Text,
    Button,
    TextInput
} from 'react-native';

export default screen('test3', ({ params, nav }) => {
    console.log('3');
    return (
        <View>
            <Text>3: {JSON.stringify(params)}</Text>
            <Button title='nav 1' onPress={() => {
                nav.push('test1', { name: '3' });
            }}/>
            <Button title='nav 2' onPress={() => {
                nav.push('test2');
            }}/>
            <Button title='nav 3' onPress={() => {
                nav.push('test3', { param3: 3 });
            }}/>
            <Button title='nav 4' onPress={() => {
                nav.push('test4');
            }}/>
            <Button title='nav 5' onPress={() => {
                nav.push('test5');
            }}/>
            <TextInput placeholder='test 3'/>
        </View>
    )
})
