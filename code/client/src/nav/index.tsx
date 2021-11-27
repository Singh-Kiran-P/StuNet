import { createMaterialBottomTabNavigator } from '@react-navigation/material-bottom-tabs';
import React, { useEffect, useState, useContext, createContext } from 'react';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { useFocusEffect } from '@react-navigation/native';
import Store from 'react-native-encrypted-storage';

import { Login, Register } from '@/auth';
import * as options from '@/nav/routes';
import { Component } from '@/nav/types';
import { Loader } from '@/components';
import { useAnimate } from '@/util';
import header from '@/nav/header';
import Screen from '@/nav/screen';
import screens from '@/screens';

const Stack = createNativeStackNavigator();
const Tab = createMaterialBottomTabNavigator();

let hide: (hide: any) => void;
const Components = screens.map(screen => {
    let name = Object.keys(screen)[0] as keyof typeof options.s;
    return [name, Component(({ params: { tabs }, nav }, props: any) => {
        useFocusEffect(() => hide(nav.getState().index && !tabs));
        return Screen({ children: screen[name](props), ...props });
    })] as [typeof name, ReturnType<typeof Component>];
})

const Screens = Object.keys(options.t).map(() => Components.map(([name, screen], i) => {
    return <Stack.Screen
        initialParams={options.s[name]}
        component={screen}
        name={name}
        key={i}
    />
}))

const Stacks = Object.values(options.t).map((tab, i) => {
    return <Stack.Navigator
        screenOptions={{
            animationTypeForReplace: 'push',
            animation: 'fade_from_bottom',
            header: header as any
        }}
        initialRouteName={tab.screen}
        children={Screens[i]}
    />
})

const Navigators = Stacks.map(stack => () => {
    useFocusEffect(() => {
        /* theme.colors.primary = tab.colors.primary; // TODO theme
        theme.colors.accent = tab.colors.accent; */
    })
    return stack;
});

const Tabs = Object.entries(options.t).map(([name, tab], i) => {
    return <Tab.Screen
        options={{
            tabBarColor: tab.colors.primary,
            tabBarLabel: tab.title,
            tabBarIcon: tab.icon
        }}
        component={Navigators[i]}
        name={name}
        key={i}
    />
})

const key = 'token';
type Token = [string, (token: string) => void];
const Token = createContext<Token>(['', () => {}]);
export const useToken = () => useContext(Token);

export default () => {
    const [hidden, setHidden] = useAnimate(false);
    hide = hide => setHidden(!!hide);

    const [token, setToken] = useState('');
    const [load, setLoad] = useState(true);
    console.log(token);

    useEffect(() => {
        Store.getItem(key)
            .then(token => setToken(token || ''))
            .catch(() => setToken(''))
            .finally(() => setLoad(false));
    }, [])

    const context: Token = [token, (token: string) => {
        setLoad(true);
        setToken(token);
        Store.setItem(key, token).finally(() => setLoad(false));
    }]

    return (
        <Token.Provider value={context}>
            <Loader state={load}>
                {token ? (
                    <Tab.Navigator
                        barStyle={{ height: hidden ? 0 : undefined }}
                        backBehavior='history'
                        children={Tabs}
                    />
                ) : (
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
        </Token.Provider>
    )
}
