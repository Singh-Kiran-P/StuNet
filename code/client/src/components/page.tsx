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
    title?: string;
}

class Page extends Component<Props> {
    constructor(props: Props) {
        super(props);
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

export default Page;
