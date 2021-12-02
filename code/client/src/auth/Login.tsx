import React, { Route, useState, useToken, useTheme, Style, axios } from '@/.';

import {
    View,
    Text,
    Button,
    TextInput,
    PasswordInput
} from '@/components';

export default ({ navigation }: Route) => {
    let [mail, setMail] = useState('');
    let [password, setPassword] = useState('');
    let [error, setError] = useState('');
    let [_, setToken] = useToken();
    let [theme] = useTheme();

    const s = Style.create({
        screen: {
            flex: 1,
            padding: theme.padding,
            backgroundColor: theme.background
        },
        
        header: {
            color: theme.primary,
            marginBottom: theme.padding
        },

        hint: {
            marginTop: theme.margin
        },

        margin: {
            marginBottom: theme.margin
        }
    })

    const login = () => {
        axios.post('/Auth/login', {
            email: mail,
            password: password
        }).then(res => setToken(res.data.jwtBearerToken))
        .catch(err => setError(err.response.data));
    }

    return (
        <View style={s.screen}>
            <Text style={s.header} type='header'>Login</Text>
    
            <TextInput style={s.margin} label='E-mail' onChangeText={setMail} />
            <PasswordInput style={s.margin} label='Password' onChangeText={setPassword} />
            <Text style={s.margin} type='error' visible={!!error}>{error}</Text>
    
            <Button style={s.margin} onPress={login} disabled={!login || !password}>Log in</Button>
    
            <Text style={s.hint} type='hint'>
                Don't have an account yet? <Text type='link' size='' onPress={() => navigation.navigate('Register')}>Register here!</Text>
            </Text>
        </View>
    )
}
