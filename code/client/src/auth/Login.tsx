import React, { Route, Style, useTheme, useState, useToken, axios, show } from '@/.';
import { View, Text, Button, TextInput, PasswordInput } from '@/components';

export default ({ navigation, route: { params } }: Route) => {
    let [password, setPassword] = useState('');
    let [error, setError] = useState('');
    let [email, setEmail] = useState('');
    let [_, setToken] = useToken();
    let [theme] = useTheme();

    const s = Style.create({
        screen: {
            backgroundColor: theme.background
        }
    })

    const login = () => {
        setError('');
        axios.post('/Auth/login', {
            email: email,
            password: password
        }).then(res => setToken(res.data.jwtBearerToken || ''), show(setError))
    }

    return (
        <View style={s.screen} padding flex>
            <Text type='title' children='Log in'/>
            <Text type='hint' margin='bottom' hidden={!params?.registered}>
                You must first confirm the verification email sent to{'\n'}
                <Text type='link' size='auto' children={params?.registered || ''}/>{'\n'}
                before you can log in.
            </Text>
            <TextInput label='Email' onChangeText={setEmail}/>
            <PasswordInput margin label='Password' onChangeText={setPassword}/>
            <Text type='error' margin hidden={!error} children={error}/>
            <Button margin children='Log in' disabled={!email || !password} toggled={error} onPress={login}/>
            <Text type='hint' margin>
                Don't have an account yet?{' '}
                <Text type='link' size='auto' onPress={() => navigation.navigate('Register')}>
                    Register here!
                </Text>
            </Text>
        </View>
    )
}
