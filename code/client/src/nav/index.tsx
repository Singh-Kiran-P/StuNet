import { createMaterialBottomTabNavigator } from '@react-navigation/material-bottom-tabs';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { useFocusEffect } from '@react-navigation/native';
import React, { useEffect } from 'react';

import { Component, Params } from '@/nav/types';
import ThemeProvider, { Theme } from '@/css';
import { useAnimate, Route } from '@/util';
import * as options from '@/nav/routes';
import { contains } from '@/util/alg';
import header from '@/nav/header';
import Screen from '@/nav/screen';
import screens from '@/screens';

const Stack = createNativeStackNavigator();
const Tab = createMaterialBottomTabNavigator();

const updates = Array<Route['navigation']>(Object.keys(options.t).length);
export const update = <T extends keyof typeof options.s>(name: T, params?: Partial<Params[T]>) => {
    updates.forEach(tab => {
        let state = tab.getState();
        state.routes.forEach(route => {
            if (route.name !== name) return;
            if (!contains(route.params, params)) return;
            tab.dispatch({
                type: 'SET_PARAMS',
                payload: { params: {} },
                source: route.key,
                target: state.key
            })
        })
    })
}

let hide: (hide: any) => void;
const Components = screens.map(screen => {
    let name = Object.keys(screen)[0] as keyof typeof options.s;
    return [name, Component(({ params: { tabs }, nav }, props: any) => {
        console.log(name);
        useEffect(() => { updates[nav.getParent()?.getState().index || 0] = nav; }, []);
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
    return (
        <ThemeProvider {...Theme.tabs[tab.colors]}>
            <Stack.Navigator
                screenOptions={{
                    animationTypeForReplace: 'push',
                    animation: 'fade_from_bottom',
                    header: header as any
                }}
                children={Screens[i]}
                initialRouteName={tab.screen}
            />
        </ThemeProvider>
    )
})

const Navigators = Stacks.map(stack => () => stack);

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
