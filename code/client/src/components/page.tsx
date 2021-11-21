import React, { Component } from 'react';
import { Theme } from '@/css';

import {
    View,
    ScrollView,
    StyleSheet
} from 'react-native';

import {
    Appbar
} from 'react-native-paper';

type Props = {
    title?: string,
    children?: React.ReactNode,
}

export default function Page(props: Props) {
    return (
        <View style={s.page}>
            <Appbar.Header>
                <Appbar.BackAction onPress={() => {}} />
                <Appbar.Content title={props.title || ''} />
            </Appbar.Header>
            <ScrollView contentContainerStyle={s.view}>
                {props.children}
            </ScrollView>
        </View>
    )
}

const s = StyleSheet.create({
    page: {
        flex: 1,
    },

    view: {
        padding: Theme.padding
    }
});