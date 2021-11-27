<<<<<<< HEAD:code/client/src/auth/Login.tsx
import React, { useState, axios, Style } from '@/.';
import { Auth } from '@/nav/types';

import {
    View,
    Text,
    Button,
    TextInput,
    PasswordInput
} from '@/components';

=======
import React, { Screen, useState, axios } from '@/.';
import { Text, View, TextInput, Button, PasswordInput } from '@/components';
>>>>>>> e4c60ba7f4192eadb204cb1984f639d0f49140e9:code/client/src/screens/auth/Login.tsx
import { HelperText } from 'react-native-paper';

export default ({ navigation }: Auth) => {
    const [mail, setMail] = useState('');
    const [password, setPassword] = useState('');
    const [errMessage, setErrMessage] = useState('');

    const login = () => {
        axios.post('/Auth/login', {
            email: mail,
            password: password
        })
        .then(res => {}) // TODO
        .catch(err => { setErrMessage(err.response.data) });
    }

    return (
        <View>
            <TextInput label='E-mail' onChangeText={setMail} />
            <PasswordInput label='Password' onChangeText={setPassword} showable={true} />
            <HelperText type='error' visible={errMessage !== ''}>{errMessage}</HelperText>
            <Button onPress={login} disabled={!login || !password}>Log in</Button>
        </View>
    )
}
