import React, { Component } from 'react';
import Page from '@components/page';
import { text } from '@css';

import {
    Button,
    TextInput
} from 'react-native-paper';

import {
    Text
} from 'react-native';

class AskQuestion extends Component {
    render = () => (
        <Page title="Ask Question">
            <Text style={[text.header]}>Course, Subject</Text>
            <TextInput mode='outlined' label='Title' />
            <TextInput mode='outlined' label='Content' multiline numberOfLines={5} />
            <Text>TODO TOPICS FILES, MORE?</Text>
            <Button mode='contained' onPress={() => {}}>Text</Button>
        </Page>
    )
}

export default AskQuestion;
