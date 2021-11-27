import React, { Screen, useState, axios } from '@/.';
import { Text, View, TextInput, Button, PasswordInput } from '@/components';
import { HelperText } from 'react-native-paper';

export default Screen('Login', ({ params, nav }) => {
    const [mail, setMail] = useState('');
    const [password, setPassword] = useState('');
    const [errMessage, setErrMessage] = useState('');

    const login = () => {
        axios.post('/Auth/login', {
            email: mail,
            password: password
        })
        .then(res => nav.navigate('TabHome'))
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
})
