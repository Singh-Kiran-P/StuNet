import { createMaterialBottomTabNavigator } from '@react-navigation/material-bottom-tabs';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { useFocusEffect } from '@react-navigation/native';
import React from 'react';

import * as options from '@/nav/routes';
import { Component } from '@/nav/types';
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
    return (
        <Stack.Screen key={i} name={name}
        initialParams={options.s[name]}
        component={screen}/>
    )
}))

const Stacks = Object.values(options.t).map((tab, i) => () => {
    let theme = useTheme();
    theme.colors.primary = tab.colors.primary; // TODO
    theme.colors.accent = tab.colors.accent;
    return (
        <Stack.Navigator initialRouteName={tab.screen} screenOptions={{
            header: header as any,
            animationTypeForReplace: 'push',
            animation: 'fade_from_bottom'
        }} children={Screens[i]}/>
    )
})

const Tabs = Object.entries(options.t).map(([name, tab], i) => {
    return (
        <Tab.Screen key={i} name={name}  options={{
            tabBarLabel: tab.title,
            tabBarIcon: tab.icon,
            tabBarColor: tab.colors.primary
        }} component={Stacks[i]}/>
    )
})

let hide: (hide: any) => void;
const main = () => {
    const [hidden, setHidden] = useAnimate(false);
    hide = hide => setHidden(!!hide);
    return <Tab.Navigator barStyle={{
        height: hidden ? 0 : undefined
    }} backBehavior='history' children={Tabs}/>
}

const auth = () => {
    // TODO
}

export default main; // TODO
