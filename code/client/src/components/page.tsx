import React, { Component } from 'react';
import { Theme } from 'css';

import {
    Appbar
} from 'react-native-paper';

import {
    View,
    StyleSheet,
    ScrollView
} from 'react-native';

type Props = {
    title?: string;
}

export default class Page extends Component<Props> {
    constructor(props: Props) {
        super(props)
    }

    render = () => (
        <View>
            <Appbar.Header>
                <Appbar.BackAction onPress={() => {}} />
                <Appbar.Content title={this.props.title || ''} />
            </Appbar.Header>
            <ScrollView style={s.view}>
                {this.props.children}
            </ScrollView>
        </View>
    )
}

const s = StyleSheet.create({
    view: {
        padding: Theme.padding
    }
});
