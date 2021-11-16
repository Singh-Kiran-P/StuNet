import React, { Component } from 'react';

import CheckboxItem from '@components/CheckboxItem';
import Page from '@components/page';
import Chip from '@components/Chip';
import { text } from '@css';

import {
    View,
    LayoutAnimation,
    Text
} from 'react-native';

import {
    Button,
    List,
    TextInput,
} from 'react-native-paper';

class AskQuestion extends Component {
    topics = [
        'Topic 1',
        'Topic 2',
        'Topic 3',
        'Topic 4'
    ];

    checks: boolean[] = this.topics.map(() => false);

    render = () => (
        <Page title='Ask Question'>
            <Text style={[text.header]}>Course, Subject</Text>
            <TextInput mode='outlined' label='Title' />
            <TextInput mode='outlined' label='Content' multiline numberOfLines={5} />

            <List.Accordion title='Topics' onPress={() => { LayoutAnimation.easeInEaseOut() }}>
                {this.topics.map((item, i) => {
                    return <CheckboxItem key={i} label={item} checked={() => this.checks[i]} oncheck={checked => this.checks[i] = checked} />
                })}
            </List.Accordion>

            <Text>TODO TOPICS FILES, MORE?</Text>
            <Button mode='contained' onPress={() => console.log(this.checks)}>Ask</Button>
        </Page>
    )
}

export default AskQuestion;
