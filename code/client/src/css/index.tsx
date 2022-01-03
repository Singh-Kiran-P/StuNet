import React, { useState, useMemo, useContext, createContext, useLayoutEffect } from 'react';
import { StyleSheet, useColorScheme } from 'react-native';

import { Base, Light, Dark } from '@/css/theme';
import { Children, Styling, Opt } from '@/util';
import { merge } from '@/util/alg';

export const Theme = {
    ...Base,
    light: Light,
    dark: Dark
}

const dark = true as boolean | undefined;
const tab = 'auth' as keyof typeof Base.tabs;

const Initial = {
    ...Base.tabs[tab],
    dark: dark,
    tab: tab
}

type Theme = typeof Base & typeof Initial & (typeof Light | typeof Dark);

type Context = [Theme, (set: Opt<Omit<Theme, keyof typeof Base>>) => void];
const Context = createContext<Context>([{} as any, () => {}]);
export const useTheme = () => useContext(Context);

export const theming = <T extends Styling>(style: (theme: Theme) => T) => {
    let [theme] = useTheme();
    return StyleSheet.create({
        ['']: style(theme)
    })['']
}

export const paper = (theme: Theme) => {
    return {
        dark: !!theme.dark,
        mode: 'exact' as 'exact',
        roundness: theme.radius,
        colors: {
            error: theme.error,
            accent: theme.accent,
            primary: theme.primary,
            text: theme.foreground,
            disabled: theme.disabled,
            onSurface: theme.surface,
            surface: theme.foreground,
            notification: theme.accent,
            backdrop: theme.placeholder,
            background: theme.background,
            placeholder: theme.placeholder
        }
    }
}

type Props = (typeof Base.tabs)[keyof typeof Base.tabs];
export default ({ children, ...tab }: Children & Props) => {
    let [theme, setTheme] = useState({
        ...(Initial.dark ? Dark : Light),
        ...Initial,
        ...tab
    })

    const context = useMemo<Context>(() =>
        [theme, (set: Opt<Theme>) => {
            if ('dark' in set) set = { ...(set.dark ? Dark : Light), ...set };
            if ('tab' in set) set = { ...Base.tabs[set.tab!], ...set };
            setTheme(merge(theme, set) as any);
        }
    ], [theme])

    let scheme = useColorScheme();
    useLayoutEffect(() => {
        let dark = scheme === 'dark';
        if (dark !== theme.dark) context[1]({ dark: dark });
    }, [scheme])

    return <Context.Provider value={context} children={children}/>
}
