import React, { useEffect, useState, useMemo, useContext, createContext } from 'react';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import Store from 'react-native-encrypted-storage';
import jwt_decode from 'jwt-decode';
import axios from 'axios';

import ThemeProvider, { Theme } from '@/css';
import Register from '@/auth/Register';
import { Loader } from '@/components';
import { Children } from '@/util';
import Login from '@/auth/Login';

const Stack = createNativeStackNavigator();

const key = 'token';
type Context = [string, (token: string) => void];
const Context = createContext<Context>(['', () => {}]);
export const useToken = () => useContext(Context);
export const useEmail = (): string => {
    let [token] = useToken();
    if (!token) return '';
    return (jwt_decode(token) as any).username || '';
};

export default ({ children }: Children) => {
    const [token, setToken] = useState('');
    const [load, setLoad] = useState(true);

    axios.defaults.headers.common['Authorization'] = 'Bearer ' + token;

    useEffect(() => {
        Store.getItem(key).then(
            token => {
                if (!token) throw Error();
                axios.get('/Auth/validateToken', { params: { token: token } }).then(
                    () => setToken(token || ''),
                    () => setToken('')
                ).finally(() => setLoad(false));
            }).catch(() => (setToken(''), setLoad(false)));
    }, [])

    const context = useMemo<Context>(() =>
        [token, token => {
            setLoad(true);
            setToken(token);
            Store.setItem(key, token).finally(
                () => setLoad(false)
            )
        }
    ], [token])

    return (
        <ThemeProvider {...Theme.tabs.auth}>
            <Context.Provider value={context}>
                <Loader state={load}>
                    {token ? children : (
                        <Stack.Navigator
                            screenOptions={{
                                animationTypeForReplace: 'push',
                                animation: 'fade_from_bottom',
                                headerShown: false
                            }}>
                            <Stack.Screen name='Login' component={Login}/>
                            <Stack.Screen name='Register' component={Register}/>
                        </Stack.Navigator>
                    )}
                </Loader>
            </Context.Provider>
        </ThemeProvider>
    )
}
