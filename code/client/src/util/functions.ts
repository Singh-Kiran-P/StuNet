import { LayoutAnimation } from 'react-native';
import { useState } from 'react';
import { Props } from '@/util';

export const animate = LayoutAnimation.easeInEaseOut;

export const useAnimate = <S>(initialState: S | (() => S)): [S, (value: S | ((prevState: S) => S)) => void] => {
    const state = useState(initialState);
    return [state[0], a => (animate(), state[1](a))];
}

export const extend = <T extends React.JSXElementConstructor<any>, U extends {} = {}>(c: T, e: (p: Props<T> & U) => JSX.Element) => {
    return Object.assign(e as (p: Partial<Props<T>> & U) => JSX.Element, c);
}
