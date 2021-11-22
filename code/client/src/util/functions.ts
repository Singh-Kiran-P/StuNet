import { LayoutAnimation } from 'react-native';
import { useState } from 'react';


export const animate = LayoutAnimation.easeInEaseOut;

export const useAnimate = <S>(initialState: S | (() => S)): [S, (value: S | ((prevState: S) => S)) => void] => {
    const state = useState(initialState);
    return [state[0], a => (animate(), state[1](a))];
}
