import React, { extend } from '@/.';
import { Props } from '@/components/extend';
import { SectionList, SectionListProps } from 'react-native';

type ItemList = <T>(props: SectionListProps<T> & Props) => JSX.Element

export default extend(SectionList, props => {
    return <SectionList
        {...props}
    />
}) as ItemList;
