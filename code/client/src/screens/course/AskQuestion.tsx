import React, { Screen, useState, axios, Topic } from '@/.';
import { Text, Button, Loader, Checkbox, Collapse, TextInput } from '@/components';

export default Screen('AskQuestion', ({ params, nav }) => {
    const [header, setHeader] = useState('');
    const [topics, setTopics] = useState<[Topic, boolean][]>([]);
    const [title, setTitle] = useState('');
    const [body, setBody] = useState('');

    const fetch = async () => { // TODO get from route
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
        }).then(() => nav.pop())
        .catch(err => {}); // TODO handle error
    }

    return (
        <Loader load={fetch}>
            <Text type='header' children={header}/>
            <TextInput margin label='Title' onChangeText={setTitle}/>
            <TextInput margin label='Body' multiline onChangeText={setBody}/>
            <Collapse margin title='Topics'>
                {topics.map(([{ name }, value], i) => (
                    <Checkbox.Item key={i} mode='ios' label={name} status={value ? 'checked' : 'unchecked'} onPress={() => {
                        setTopics(topics.map(([n, v], j) => i === j ? [n, !v] : [n, v]));
                    }}/>
                ))}
            </Collapse>
            <Button margin children='Ask' disabled={!title || !body} onPress={submit}/>
        </Loader>
    )
})
