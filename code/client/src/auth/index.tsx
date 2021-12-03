import React, { useEffect, useState, useMemo, useContext, createContext } from 'react';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import Store from 'react-native-encrypted-storage';
import axios from 'axios';

import ThemeProvider, { Theme } from '@/css';
import { Children } from '@/util';
import { Loader } from '@/components';
import Register from '@/auth/Register';
import Login from '@/auth/Login';

const Stack = createNativeStackNavigator();

const key = 'token';
type Context = [string, (token: string) => void];
const Context = createContext<Context>(['', () => {}]);
export const useToken = () => useContext(Context);

export default ({ children }: Children) => {
    const [token, setToken] = useState('');
    const [load, setLoad] = useState(true);

    axios.defaults.headers.common['Authorization'] = 'Bearer ' + token;

    useEffect(() => {
        Store.getItem(key)
            .then(token => setToken(token || ''))
            .catch(() => setToken(''))
            .finally(() => setLoad(false));
    }, [])

    const context = useMemo<Context>(() =>
        [token, token => {
            setLoad(true);
            setToken(token);
            Store.setItem(key, token)
                .finally(() => setLoad(false));
        }
    ], [token])

    return (
        <Context.Provider value={context}>
            <Loader state={load}>
                {token ? children : (
                    <ThemeProvider {...Theme.tabs.auth}>
                        <Stack.Navigator
                            screenOptions={{
                                animationTypeForReplace: 'push',
                                animation: 'fade_from_bottom',
                                headerShown: false
                            }}>
                            <Stack.Screen name='Login' component={Login}/>
                            <Stack.Screen name='Register' component={Register}/>
                        </Stack.Navigator>
                    </ThemeProvider>
                )}
            </Loader>
        </Context.Provider>
    )
}
