import { StyleSheet, ViewStyle, TextStyle, ImageStyle, useColorScheme } from 'react-native';
import React, { useState, useMemo, useContext, createContext, useEffect } from 'react';

import { Base, Light, Dark } from '@/css/theme';
import { Children } from '@/util';

export const Theme = {
    ...Base,
    light: Light,
    dark: Dark
}

const Initial = {
    dark: undefined as boolean | undefined,
    primary: Base.tabs.auth.primary,
    accent: Base.tabs.auth.accent
}

type Theme = typeof Base & typeof Initial & (typeof Light | typeof Dark);

type Obj = Object & { [k: string]: any };
type Opt<T> = { [U in keyof T]?: Opt<T[U]> };
const obj = (o: any) => o && typeof o === 'object' && !Array.isArray(o);
const merge = (a: Obj, b: Obj) => Object.keys(a).reduce((c, k) => {
    if (!b.hasOwnProperty(k)) c[k] = a[k];
    else if (!obj(b[k])) c[k] = b[k];
    else c[k] = merge(a[k], b[k]);
    return c;
}, {} as Obj);

type Context = [Theme, (set: Opt<Omit<Theme, keyof typeof Base>>) => void];
const Context = createContext<Context>([{} as any, () => {}]);
export const useTheme = () => useContext(Context);

type Style = ViewStyle | TextStyle | ImageStyle;
export const theming = <T extends Style>(style: (theme: Theme) => T) => {
    let [theme] = useTheme();
    return StyleSheet.create({
        ['']: style(theme)
    })['']
}

export const paper = (theme: Theme) => {
    return {
        dark: !!theme.dark,
        mode: 'exact' as 'exact',
        colors: {
            error: theme.error,
            accent: theme.accent,
            primary: theme.primary,
            text: theme.foreground,
            disabled: theme.disabled,
            surface: theme.foreground,
            backdrop: theme.placeholder,
            onSurface: theme.background,
            background: theme.background,
            placeholder: theme.placeholder,
            notification: theme.notification
        }
    }
}

export default ({ children }: Children) => {
    let [theme, setTheme] = useState({
        ...(Initial.dark ? Dark : Light),
        ...Initial
    })

    const context = useMemo<Context>(() =>
        [theme, (set: Opt<Theme>) => {
            if ('dark' in set) set = { ...(set.dark ? Dark : Light), ...set };
            setTheme(merge(theme, set) as any);
        }
    ], [theme])

    let scheme = useColorScheme();
    useEffect(() => {
        if (scheme === theme.dark) return;
        context[1]({ dark: scheme === 'dark' });
    }, [scheme])

    return <Context.Provider value={context} children={children}/>
}
