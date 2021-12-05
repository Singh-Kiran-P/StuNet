import React, { Route, Style, useTheme, useState, useToken, axios, errorString } from '@/.';
import { View, Text, Button, TextInput, PasswordInput } from '@/components';

export default ({ navigation }: Route) => {
    let [password, setPassword] = useState('');
    let [error, setError] = useState('');
    let [email, setEmail] = useState('');
    let [_, setToken] = useToken();
    let [theme] = useTheme();

    const s = Style.create({
        screen: {
            padding: theme.padding,
            backgroundColor: theme.background
        },

        header: {
            color: theme.primary,
            marginBottom: theme.padding
        }
    })

    const login = () => {
        axios.post('/Auth/login', {
            email: email,
            password: password
        }).then(res => setToken(res.data.jwtBearerToken || ''))
        .catch(err => setError(errorString(err)));
    }

    return (
        <View style={s.screen} flex>
            <Text style={s.header} type='header' children='Log in'/>
    
            <TextInput label='Email' onChangeText={setEmail}/>
            <PasswordInput margin label='Password' onChangeText={setPassword}/>
            <Text margin type='error' hidden={!error} children={error}/>
    
            <Button margin children='Log in' disabled={!login || !password} onPress={login}/>
    
            <Text margin type='hint'>
                Don't have an account yet?{' '}
                <Text type='link' size='auto' onPress={() => navigation.navigate('Register')}>
                    Register here!
                </Text>
            </Text>
        </View>
    )
}
