import React from 'react';

import screens from '@/screens';
import * as options from '@/nav/routes';

const tabs = {
    // TODO
}

import { createMaterialBottomTabNavigator } from '@react-navigation/material-bottom-tabs';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
const Tab = createMaterialBottomTabNavigator();
const Stack = createNativeStackNavigator();

const Screens = undefined;

const Stacks = undefined;

const Tabs = undefined;

export default () => <Tab.Navigator children={Tabs}/>;

/* 
let screens = Object.assign({}, ...elements.map(el => {
    let name = Object.keys(el)[0] as Name;
    let element = el[name];
    return { [name]: {
        el: element,
        options: options[name]
    }}
}));

const stacks = Object.assign({}, ...Object.entries(routes).map(([route, tab]) => ({
    [route]: () => {
        console.log('b:   ' + Math.random());
        return (
            <Stack.Navigator>
                {tab.screens.map(s => [s, screens[s]]).map(([s, screen], i) => {
                    console.log('c:         ' + Math.random());
                    return <Stack.Screen key={i} name={s} component={screen.el}/>
                })}
            </Stack.Navigator>
        )
    }
})));

const tabs = Object.entries(routes).map(([route, tab], i) => {
    console.log('a:' + Math.random());
    return (
        <Tab.Screen key={i} name={route} options={{
            tabBarLabel: tab.title,
            tabBarIcon: tab.icon
        }} component={stacks[route]}/>
    )
});
*/
