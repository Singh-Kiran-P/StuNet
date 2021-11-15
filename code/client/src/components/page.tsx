import React, { Component } from 'react';
import { Theme } from 'css';

import {
    Appbar
} from 'react-native-paper';

import {
    StyleSheet,
    View
} from 'react-native';

type Props = {
    title?: string;
}

class Page extends Component<Props> {
    constructor(props: Props) {
        super(props)
    }

    render = () => (
        <View>
            <Appbar.Header>
                <Appbar.BackAction onPress={() => {}} />
                <Appbar.Content title={this.props.title || ''} />
            </Appbar.Header>
            <View style={s.view}>
                {this.props.children}
            </View>
        </View>
    )
}

const s = StyleSheet.create({
    view: {
        padding: Theme.padding
    }
});

export default Page;
