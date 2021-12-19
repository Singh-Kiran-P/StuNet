import React, { extend } from '@/.';
import { Props } from '@/components/extend';
import { FlatList, FlatListProps } from 'react-native';
import { List } from 'react-native-paper';

type ItemList = <T>(props: FlatListProps<T> & Props) => JSX.Element

export default Object.assign(extend(FlatList, props => {
    return <FlatList
        {...props}
    />
}) as ItemList, List)
