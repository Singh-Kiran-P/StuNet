import React, { useEffect, useState } from 'react';
import CheckboxItem from '@components/CheckboxItem';
import Page from '@components/page';
import { text } from '@css';

import {
    LayoutAnimation,
    Text
} from 'react-native';

import {
    ActivityIndicator,
    Button,
    List,
    TextInput,
} from 'react-native-paper';

type Topic = {
    id: number,
    name: string
}

export default function AskQuestion() {
    const [loading, setLoading] = useState(true);
    
    const [title, setTitle] = useState('')
    const [body, setBody] = useState('')
    const [topics, setTopics] = useState<Topic[]>([]);
    const [checks, setChecks] = useState<boolean[]>([]);

    useEffect(() => {
        setLoading(true)
        fetch('http://10.0.2.2:5000/Topic', {
            method: 'GET'
        }).then(res => res.json())
            .then(json => setTopics(json));
        setChecks(topics.map(() => false));
        setLoading(false);
    }, []);

    const submit = () => {
        console.log(title);
        console.log(body);
        console.log(checks.map((checked, i) => checked ? topics[i].id : null).filter(x => x !== null));
    };

    return loading ?
        <ActivityIndicator size='large' style={{ flex: 1 }}/>
        :
        <Page title='Ask Question'>
            <Text style={[text.header]}>Course, Subject</Text>
            <TextInput mode='outlined' label='Title' onChangeText={setTitle} />
            <TextInput mode='outlined' label='Content' multiline numberOfLines={5} onChangeText={setBody} />
            <List.Accordion title='Topics' onPress={() => { LayoutAnimation.easeInEaseOut() }}>
                {topics.map(({id, name}, i) => {
                    return <CheckboxItem key={id} label={name} checked={() => checks[i]} oncheck={checked => checks[i] = checked} />
                })}
            </List.Accordion>
            <Button mode='contained' onPress={submit} disabled={title === '' || body === ''}>Ask</Button>
        </Page>
}
