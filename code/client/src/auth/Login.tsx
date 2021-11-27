import React, { useState, axios, useToken } from '@/.';
import { Navigation } from '@/nav/types';

import {
    View,
    Text,
    Button,
    TextInput,
    PasswordInput
} from '@/components';

export default ({ navigation }: Navigation) => {
    const [mail, setMail] = useState('');
    const [password, setPassword] = useState('');
    const [errMessage, setErrMessage] = useState('');
    const [_, setToken] = useToken();

    const login = () => {
        axios.post('/Auth/login', {
            email: mail,
            password: password
        }).then(res => {
            let token = 'TODO-REAL-TOKEN';
            setToken(token);
        }).catch(err => { setErrMessage(err.response.data) });
    }

    return (
        <View>
            <TextInput label='E-mail' onChangeText={setMail} />
            <PasswordInput label='Password' onChangeText={setPassword} showable={true} />
            <Text type='error' visible={errMessage !== ''}>{errMessage}</Text>
            <Button onPress={login} disabled={!login || !password}>Log in</Button>
            <Button onPress={() => navigation.replace('Register')}>Register</Button>
        </View>
    )
}
