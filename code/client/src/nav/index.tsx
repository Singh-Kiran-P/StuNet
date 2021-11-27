import { createMaterialBottomTabNavigator } from '@react-navigation/material-bottom-tabs';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { useFocusEffect } from '@react-navigation/native';
import React, { createContext, useState } from 'react';

import { Login, Register } from '@/auth';
import * as options from '@/nav/routes';
import { Component } from '@/nav/types';
import { Loader } from '@/components';
import { useAnimate } from '@/util';
import header from '@/nav/header';
import Screen from '@/nav/screen';
import screens from '@/screens';
import { useTheme } from 'react-native-paper';

const Stack = createNativeStackNavigator();
const Tab = createMaterialBottomTabNavigator();

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

const Stacks = Object.values(options.t).map((tab, i) => () => {
    let theme = useTheme();
    theme.colors.primary = tab.colors.primary; // TODO
    theme.colors.accent = tab.colors.accent;
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

const Tabs = Object.entries(options.t).map(([name, tab], i) => {
    return <Tab.Screen
        options={{
            tabBarColor: tab.colors.primary,
            tabBarLabel: tab.title,
            tabBarIcon: tab.icon
        }}
        component={Stacks[i]}
        name={name}
        key={i}
    />
})

import { View } from 'react-native';

let hide: (hide: any) => void;
export default () => {
    const [token, setToken] = useState<string | null>(null);

    const getToken = async () => { // TODO
        return new Promise<void>((res, rej) => {
            setTimeout(() => res(), 1000);
        }).then(token => setToken(''));
    }

    const [hidden, setHidden] = useAnimate(false);
    hide = hide => setHidden(!!hide);

    return <Loader load={getToken}>
        <View style={{ width: '100%', height: '100%' }}>
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
        </View>
    </Loader>
}
