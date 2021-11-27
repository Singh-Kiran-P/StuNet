import React, { Screen, useState, axios } from '@/.';

import {
    Text,
    Button,
    Loader,
    Checkbox,
    Collapse,
    TextInput
} from '@/components';

type Topic = {
    id: number,
    name: string
}

export default Screen('AskQuestion', ({ params, nav }) => {
    const [header, setHeader] = useState('');
    const [topics, setTopics] = useState<[Topic, boolean][]>([]);
    const [title, setTitle] = useState('');
    const [body, setBody] = useState('');

    const fetch = async () => {
        return axios.get('/Course/' + params.courseId)
            .then(res => {
                setHeader(res.data.name);
                setTopics(res.data.topics.map((t: { id: number; name: string; }) => [{ id: t.id, name: t.name }, false]));
            })
    }

    const submit = () => {
        axios.post('/Question', {
            courseId: params.courseId,
            title: title,
            body: body,
            topicIds: topics.filter(topic => topic[1]).map(topic => topic[0].id),
        }).then(res => nav.navigate('Question', { id: res.data.id }))
        .catch(err => {}); // TODO handle error
    }

    return (
        <Loader load={fetch}>
            <Text mode='header' children={header}/>
            <TextInput label='Title' onChangeText={setTitle}/>
            <TextInput label='Body' multiline onChangeText={setBody}/>
            <Collapse title='Topics'>
                {topics.map(([{ name }, value], i) =>
                    <Checkbox.Item key={i} mode='ios' label={name} status={value ? 'checked' : 'unchecked'} onPress={() => {
                        setTopics(topics.map(([n, v], j) => i === j ? [n, !v] : [n, v]));
                    }}/>
                )}
            </Collapse>
            <Button children='Ask' disabled={!title || !body} onPress={submit}/>
        </Loader>
    )
})
