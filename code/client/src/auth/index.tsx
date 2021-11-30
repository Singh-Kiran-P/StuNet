import React, { useEffect, useState, useMemo, useContext, createContext, useLayoutEffect } from 'react';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import Store from 'react-native-encrypted-storage';

import { useTheme } from '@/css';
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
    let [_, setTheme] = useTheme();

    useEffect(() => {
        Store.getItem(key)
            .then(token => setToken(token || 'TODO-EMPTY-STRING'))
            .catch(() => setToken(''))
            .finally(() => setLoad(false));
    }, [])

    useLayoutEffect(() => {
        if (!token) setTheme({ tab: 'auth' });
    }, [token])

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
    )
}
