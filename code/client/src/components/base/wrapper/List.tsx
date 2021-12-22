import React, { extend } from '@/.';
import { FlatList, FlatListProps } from 'react-native';
import Spinner from '@/components/base/Spinner';
import { Props } from '@/components/extend';
import { List } from 'react-native-paper';

type ItemList = <T>(props: FlatListProps<T> & Props) => JSX.Element;

export default Object.assign(extend(FlatList, ({ refreshing, ...props }) => {
    if (refreshing) return <Spinner/>
    return <FlatList
        {...props}
    />
}) as ItemList, List);
