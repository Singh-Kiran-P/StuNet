import { LayoutAnimation } from 'react-native';
import { useState } from 'react';

export const animate = LayoutAnimation.easeInEaseOut;

export const useAnimate = <S>(initialState: S | (() => S)): [S, (value: S | ((prevState: S) => S)) => void] => {
    const state = useState(initialState);
    return [state[0], a => (animate(), state[1](a))];
}

export const dateString = (date: any) => new Date(date).toLocaleString();

export const errorString = (err: any): string => {
    const v = (a?: any): string => {
        if (typeof a === 'object') return v(Object.values(a)[0]);
        return `${a}` || 'An unknown error has occurred';
    }
    err = err?.response?.data;
    if (typeof err !== 'object') return v(err);
    if (Array.isArray(err)) err = err[0] || {};
    if (err.description) return v(err.description);
    if (err.errors) return v(err.errors);
    return v();
}
