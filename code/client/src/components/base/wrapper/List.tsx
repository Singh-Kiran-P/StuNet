import React, { extend } from '@/.';
import { FlatList, FlatListProps } from 'react-native';
import { Props } from '@/components/extend';
import { List } from 'react-native-paper';

type ItemList = <T>(props: FlatListProps<T> & Props) => JSX.Element;

// TODO scroll indicator mode change

export default Object.assign(extend(FlatList, props => {
    return <FlatList
        {...props}
    />
}) as ItemList, List);
