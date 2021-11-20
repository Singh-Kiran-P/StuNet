type BaseScreens = {
    [name: string]: {
        args?: object;
        title: string | ((a: any) => string);
        padding?: boolean | number;
        scroll?: boolean;
        // TODO color? ...?
    }
}

type BaseTabs = {
    [name: string]: {
        title: string;
        icon: string;
        // TODO color? ...?
    }
}

const t = <T extends BaseTabs>(v: T) => v;
const s = <T extends BaseScreens>(v: T) => v;
export { s as screens, t as tabs };

import { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { MaterialBottomTabNavigationProp } from '@react-navigation/material-bottom-tabs';
import { RouteProp, CompositeNavigationProp } from '@react-navigation/native';
import { s as screens, t as tabs } from '@/nav/routes';

// NavigatorScreenParams from '@react-navigation/native';
type NavigatorScreenParams<T> = { screen?: never; params?: never; initial?: never; } | {
    [U in keyof T]: undefined extends T[U]
    ? { screen: U; params?: T[U]; initial?: boolean; }
    : { screen: U; params: T[U]; initial?: boolean; }
}[keyof T]

export type Name = keyof typeof screens;
type Screens = typeof screens & { '': BaseScreens[string] };
type Route<T extends Name | ''> = RouteProp<Screens, T>;
type Params = { [T in Name]: Screens[T] extends { args: {} }
    ? Partial<Omit<Screens[T], keyof Screens[T]['args'] | 'args'>> & Screens[T]['args']
    : Partial<Screens[T]> | undefined
}

type Nav<T extends Name | ''> = CompositeNavigationProp<
    NativeStackNavigationProp<Params, T extends '' ? Name : T>, // TODO '' ? Name
    MaterialBottomTabNavigationProp<
    Partial<{ [T in keyof typeof tabs]:
        NavigatorScreenParams<Params> &
        Partial<(typeof tabs)[T]>
    }>>
>

export const screen = <T extends Name>(t: T,
    s: (props: { params: Route<T>['params'], nav: Nav<T> }) => JSX.Element) => {
    return { [t]: (props: { route: Route<T>, navigation: Nav<T> }) => s({
        params: props.route.params,
        nav: props.navigation
    })}
}

type Used = 'params' | 'nav' | 'route' | 'navigation';
export const component = <T extends {} = {}, U extends Name | '' = ''>(
    c: (props: { params: Route<U>['params'], nav: Nav<U> } & Omit<T, Used>) => JSX.Element) => {
    return (props: { route: Route<U>, navigation: Nav<U> } & Omit<T, Used>) => c(
        (({ route, navigation, ...o }) => ({ ...o,
            params: props.route.params,
            nav: props.navigation
        }))(props) as any
    )
}
