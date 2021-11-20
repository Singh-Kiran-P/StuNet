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
        <View style={s.page}>
            <Appbar.Header>
                <Appbar.BackAction onPress={() => {}} />
                <Appbar.Content title={props.title || ''} />
            </Appbar.Header>
<<<<<<< HEAD
            <ScrollView contentContainerStyle={s.view}>
                {props.children}
=======
            <ScrollView>
                <View style={s.view}>
                    {/* https://stackoverflow.com/a/59312970 */}
                    {props.children}
                </View>
>>>>>>> main
            </ScrollView>
        </View>
    )
}

const s = StyleSheet.create({
    page: {
        flex: 1,
    },

    view: {
<<<<<<< HEAD
        padding: Theme.padding
    }
=======
        padding: Theme.padding,
        flexGrow: 1,
    },
>>>>>>> main
});