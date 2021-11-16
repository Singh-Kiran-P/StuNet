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

type Form = {
    title: string,
    body: string,
    topics: string[],
    checks: boolean[],
};

export default function AskQuestion() {
    const [loading, setLoading] = useState(true);
    
    let form: Form = {
        title: "",
        body: "",
        topics: [],
        checks: []
    };

    useEffect(() => { //test loading; doesn't work for some reason
        setLoading(true)
        setTimeout(() => {
            form.topics = [
                'Topic 1',
                'Topic 2',
                'Topic 3',
                'Topic 4'
            ];
            form.checks = form.topics.map(() => false);
            setLoading(false);
        }, 1000);
    }, []);

    const submit = () => {
        console.log(form.title);
        console.log(form.body);
        console.log(form.checks.map((checked, i) => checked ? form.topics[i] : null).filter(x => x !== null));
    };

    return loading ?
        <ActivityIndicator size='large' style={{ flex: 1 }}/>
        :
        <Page title='Ask Question'>
            <Text style={[text.header]}>Course, Subject</Text>
            <TextInput mode='outlined' label='Title' onChangeText={s => form.title = s} />
            <TextInput mode='outlined' label='Content' multiline numberOfLines={5} onChangeText={s => form.body = s} />
            <List.Accordion title='Topics' onPress={() => { LayoutAnimation.easeInEaseOut() }}>
                {form.topics.map((item, i) => {
                    return <CheckboxItem key={i} label={item} checked={() => form.checks[i]} oncheck={checked => form.checks[i] = checked} />
                })}
            </List.Accordion>
            <Button mode='contained' onPress={submit}>Ask</Button>
        </Page>
}
