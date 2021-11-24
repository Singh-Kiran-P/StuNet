import React, { Screen, useState, axios, animate } from '@/.';

import {
    Text,
    Button,
    TextInput,
    LoadingWrapper
} from '@/components';

import {
    List,
    Checkbox,
} from 'react-native-paper';

type Topic = {
    id: number,
    name: string
}

export default Screen('AskQuestion', ({ params, nav }) => {
    const [header, setHeader] = useState('');
    const [topics, setTopics] = useState<[Topic, boolean][]>([]);
    const [title, setTitle] = useState('');
    const [body, setBody] = useState('');


    // On error: navigate to seperate error page? return to previous page and show error in snackbar?
    const fetch = async () => {
        return axios.get('/Course/' + params.courseId)
            .then(res => {
                setHeader(res.data.name);
                setTopics(res.data.topics.map((t: { id: number; name: string; }) => [{ id: t.id, name: t.name }, false]));
            })
            .catch(err => {})
    }

    const submit = () => {
        axios.post('/Question', {
            courseId: params.courseId,
            title: title,
            body: body,
            topicIds: topics.filter(topic => topic[1]).map(topic => topic[0].id),
        }).then(res => nav.navigate('Question', { id: res.data.id })).catch(err => {});
    }

    return (
        <LoadingWrapper func={fetch}>
            <Text mode='header'>{header}</Text>
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
        </LoadingWrapper>
    )
})
