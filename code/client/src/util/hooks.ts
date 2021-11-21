import { LayoutAnimation } from 'react-native';
import { useState } from 'react';



export const useAnimate = <S>(initialState: S | (() => S)): [S, (value: S | ((prevState: S) => S)) => void] => {
    let state = useState(initialState);
    return [state[0], a => (LayoutAnimation.easeInEaseOut(), state[1](a))];
}
