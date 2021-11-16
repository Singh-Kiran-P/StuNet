import React, { Component } from 'react';
import { View } from 'react-native';
import Page from '@components/page';
import { text, Theme } from '@css';
import CheckboxItem from '@components/CheckboxItem';
import Chip from '@components/Chip';

import {
    Button,
    List,
    TextInput,
} from 'react-native-paper';

import {
    LayoutAnimation,
    Text
} from 'react-native';

class AskQuestion extends Component {
    topics = [
        'Topic 1',
        'Topic 2',
        'Topic 3',
        'Topic 4'
    ]

    render = () => (
        <Page title="Ask Question">
            <Text style={[text.header]}>Course, Subject</Text>
            <TextInput mode='outlined' label='Title' />
            <TextInput mode='outlined' label='Content' multiline numberOfLines={5} />
            {/* Option 1: Accordion*/}
            <List.Accordion title='Topics' onPress={() => { LayoutAnimation.easeInEaseOut() }}>
                {this.topics.map((item, index) => {
                    return <CheckboxItem key={index} label={item} />
                })}
            </List.Accordion>

            {/* Option 2: Chips*/}
            {/* <View style={{flexDirection: 'row', flexWrap: 'wrap', marginVertical: 5}}>
                {this.topics.map((item, index) => {
                    return <Chip key={index}>
                        {item}
                    </Chip>
                })}
            </View> */}
            <Text>TODO TOPICS FILES, MORE?</Text>
            <Button mode='contained' onPress={() => {}}>Ask</Button>
        </Page>
    )
}

export default AskQuestion;
