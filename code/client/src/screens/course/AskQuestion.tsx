import React, { Screen, useState, axios, animate } from '@/.';

import {
    View,
    Text,
    Button,
    TextInput
} from '@/components';

import {
    List,
    Checkbox,
    ActivityIndicator // TODO
} from 'react-native-paper';

type Topic = {
    id: number,
    name: string
}

export default Screen('AskQuestion', ({ params, nav }) => { // TODO load data
    let temp = () => [...Array(8).keys()].map(i => ([{ id: i++, name: 'Topic ' + i }, false]) as [Topic, boolean]);

    const [topics, setTopics] = useState<[Topic, boolean][]>(temp);
    const [title, setTitle] = useState('');
    const [body, setBody] = useState('');


    const submit = () => {
        axios.post('/Question', {
            body: body,
            title: title,
            courseId: params.courseId,
            topics: topics.filter(topic => topic[1]).map(topic => topic[0].id),
        }).then(res => nav.navigate('Question', { id: res.data.id })).catch(err => { // TODO data.id?
            // TODO handle error
        })
    }

    return (
        <View>
            <Text mode='header'>Course, Subject</Text>
            <TextInput label='Title' onChangeText={setTitle}/>
            <TextInput label='Body' multiline onChangeText={setBody}/>
            <List.Accordion title='Topics' onPress={animate}>
                {topics.map(([{ name }, value], i) =>
                    <Checkbox.Item key={i} mode='ios' label={name} status={value ? 'checked' : 'unchecked'} onPress={() => {
                        setTopics(topics.map(([n, v], j) => i === j ? [n, !v] : [n, v]));
                    }}/>
                )}
            </List.Accordion>
            <Button children='Ask' disabled={!title || !body} onPress={submit}/>
        </View>
    )
})
