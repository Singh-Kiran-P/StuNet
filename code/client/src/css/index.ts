import { StyleSheet as Style } from 'react-native';
import { DefaultTheme } from 'react-native-paper';
import { LayoutAnimation } from 'react-native';
import { useState } from 'react';

export const Theme = {
    ...DefaultTheme,
    large: 24,
    medium: 16,
    small: 12,

    padding: 10,

    colors: {
        ...DefaultTheme.colors,

        home: {
            primary: 'blue',
            accent: 'green'
        },
        courses: {
            primary: 'green',
            accent: 'red'
        },
        notifications: {
            primary: 'red',
            accent:'purple'
        },
        profile: {
            primary: 'purple',
            accent: 'blue'
        }
    }
}

export const text = Style.create({
    header: {
        fontSize: Theme.large,
        fontWeight: 'bold'
    }
})

type Dispatch<A> = (value: A) => void;
type SetStateAction<S> = S | ((prevState: S) => S);
export const useAnimate = <S>(initialState: S | (() => S)): [S, Dispatch<SetStateAction<S>>] => {
    let state = useState(initialState);
    return [state[0], a => (LayoutAnimation.easeInEaseOut(), state[1](a))];
}
