import React, { Component } from 'react';
import { Theme } from 'css';

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
        <View>
            <Appbar.Header>
                <Appbar.BackAction onPress={() => {}} />
                <Appbar.Content title={props.title || ''} />
            </Appbar.Header>
            <ScrollView>
                <View style={s.view}>
                    {/* https://stackoverflow.com/a/59312970 */}
                    {props.children}
                </View>
            </ScrollView>
        </View>
    )
}

const s = StyleSheet.create({
    view: {
        padding: Theme.padding,
        flexGrow: 1,
    },
});