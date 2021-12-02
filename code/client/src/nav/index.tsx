import { createMaterialBottomTabNavigator } from '@react-navigation/material-bottom-tabs';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { useFocusEffect } from '@react-navigation/native';
import React, { useEffect } from 'react';

import { useAnimate } from '@/util';
import { useTheme, Theme } from '@/css';
import { Component } from '@/nav/types';
import * as options from '@/nav/routes';
import header from '@/nav/header';
import Screen from '@/nav/screen';
import screens from '@/screens';

const Stack = createNativeStackNavigator();
const Tab = createMaterialBottomTabNavigator();

let hide: (hide: any) => void;
const Components = screens.map(screen => {
    let name = Object.keys(screen)[0] as keyof typeof options.s;
    return [name, Component(({ params: { tabs }, nav }, props: any) => {
        useEffect(() => {
            // TODO ALLOW UPDATE
        }, []);
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
    return [tab, <Stack.Navigator
        screenOptions={{
            animationTypeForReplace: 'push',
            animation: 'fade_from_bottom',
            header: header as any
        }}
        children={Screens[i]}
        initialRouteName={tab.screen}
    />] as [typeof tab, JSX.Element];
})

const Navigators = Stacks.map(([tab, stack]) => () => {
    let [theme, setTheme] = useTheme();
    useFocusEffect(() => {
        if (theme.tab !== tab.colors) setTheme({ tab: tab.colors })
    });
    return stack;
})

const Tabs = Object.entries(options.t).map(([name, tab], i) => {
    return <Tab.Screen
        options={{
            tabBarColor: Theme.tabs[tab.colors].primary,
            tabBarLabel: tab.title,
            tabBarIcon: tab.icon
        }}
        component={Navigators[i]}
        name={name}
        key={i}
    />
})

export default () => {
    const [hidden, setHidden] = useAnimate(false);
    hide = hide => setHidden(!!hide);

    return <Tab.Navigator
        barStyle={{ height: hidden ? 0 : undefined }}
        backBehavior='history'
        children={Tabs}
    />
}
