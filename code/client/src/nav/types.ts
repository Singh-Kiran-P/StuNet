import { MaterialBottomTabNavigationProp } from '@react-navigation/material-bottom-tabs';
import { RouteProp, CompositeNavigationProp } from '@react-navigation/native';
import { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { useRoute, useNavigation } from '@react-navigation/core';
import { s as screens, t as tabs } from '@/nav/routes';
import { Theme } from '@/css'

type BaseScreens = {
    [name: string]: {
        args?: object;
        screenTitle: string;
        padding?: boolean | number;
        scroll?: boolean;
        tabs?: boolean;

        search?: string;
        subscribe?: boolean | null;
        logout?: boolean;

        update?: number;
    }
}

type BaseTabs<T extends BaseScreens> = {
    [name: string]: {
        screen: keyof T;
        title: string;
        icon: string;
        colors: keyof typeof Theme.tabs;
    }
}

const s = <T extends BaseScreens>(v: T): Base<BaseScreens, T> => v as any;
const t = <T extends BaseScreens, U extends BaseTabs<T>>(_: T, v: U): Base<BaseTabs<T>, U> => v as any;
type Generic<T, U extends { [key: string]: any }> = { [V in keyof U]: U[V] extends T ? T : U[V] };
type Base<T extends { [key: string]: any }, U extends T> = { [V in keyof U]: Generic<boolean | null, U[V]> & T[string] };
export { s as screens, t as tabs };

// NavigatorScreenParams from '@react-navigation/native';
type NavigatorScreenParams<T> = { screen?: never; params?: never; initial?: never; } | {
    [U in keyof T]: undefined extends T[U]
    ? { screen: U; params?: T[U]; initial?: boolean; }
    : { screen: U; params: T[U]; initial?: boolean; }
}[keyof T]

type Screens = typeof screens;
type Name = keyof typeof screens;
type Flat<T extends Name> = Omit<Screens[T], keyof Screens[T]['args'] | 'args'>;
type Route<T extends Name> = RouteProp<{ [T in Name]: Flat<T> & Screens[T]['args'] }, T>;
export type Params = { [T in Name]: Screens[T] extends { args: {} }
    ? Partial<Flat<T>> & Screens[T]['args']
    : Partial<Flat<T>> | undefined
}

type Nav<T extends Name> = CompositeNavigationProp<
    NativeStackNavigationProp<Params, T>,
    MaterialBottomTabNavigationProp<
    Partial<{ [T in keyof typeof tabs]:
        NavigatorScreenParams<Params> &
        Omit<Partial<(typeof tabs)[T]>, 'screen'>
    }>>
>

export const Screen = <T extends Name>(t: T, s:
    (args: { params: Route<T>['params'], nav: Nav<T> },
    props: { route: Route<Name>, navigation: Nav<Name> }) => JSX.Element) => {
    return { [t]: (props: { route: Route<T>, navigation: Nav<T> }) => s({
        params: props.route.params,
        nav: props.navigation
    }, props as any)}
}

type Used = 'params' | 'nav' | 'route' | 'navigation';
export const Component = <T extends {} = {}, U extends Name = Name>(c:
    (args: { params: Route<U>['params'], nav: Nav<U> } & Omit<T, Used>,
    props: { route: Route<Name>, navigation: Nav<Name> }) => JSX.Element) => {
    return (props: { route: Route<U>, navigation: Nav<U> } & Omit<T, Used>) => c(
        (({ route, navigation, ...o }) => ({ ...o,
            params: props.route.params,
            nav: props.navigation
        }))(props) as any, props as any
    )
}

export const useParams = <T extends Name = Name>() => useRoute<Route<T>>().params!;
export const useNav = <T extends Name = Name>() => useNavigation<Nav<T>>();
