import React, { extend } from '@/.';
import { SectionList, SectionListProps } from 'react-native';
import Spinner from '@/components/base/Spinner';
import { Props } from '@/components/extend';

type ItemList = <T>(props: SectionListProps<T> & Props) => JSX.Element

export default extend(SectionList, ({ refreshing, ...props }) => {
    if (refreshing) return <Spinner/>
    return <SectionList
        {...props}
    />
}) as ItemList;
