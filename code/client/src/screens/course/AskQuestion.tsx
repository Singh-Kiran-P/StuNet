import React, { Screen, useState, axios } from '@/.';
import DocumentPicker, { DocumentPickerResponse as File } from 'react-native-document-picker';
import {
    Text,
    Button,
    Loader,
    Checkbox,
    Collapse,
    TextInput
} from '@/components';
import { transformFileSync } from '@babel/core';

type Topic = {
    id: number,
    name: string
}

export default Screen('AskQuestion', ({ params, nav }) => {
    const [header, setHeader] = useState('');
    const [topics, setTopics] = useState<[Topic, boolean][]>([]);
    const [title, setTitle] = useState('');
    const [body, setBody] = useState('');
    const [file, setFile] = useState<File[] | []>([]);

    const selectFile = async () => {
        DocumentPicker.pickMultiple({ type: DocumentPicker.types.allFiles, presentationStyle: undefined }).then(res => {
            if (!file) return; // TODO handle error
            setFile(res);
        }).catch(err => {}); // TODO handle error
    }

    const fetch = async () => {
        return axios.get('/Course/' + params.courseId)
            .then(res => {
                setHeader(res.data.name);
                setTopics(res.data.topics.map((t: { id: number; name: string; }) => [{ id: t.id, name: t.name }, false]));
            })
    }

    const submit = () => {
        const data = new FormData();
        data.append('courseId', params.courseId);
        data.append('title', title);
        data.append('body', body);
        if(topics.filter(topic => topic[1]).map(topic => topic[0].id).length !=0){
            for(let id of topics.filter(topic => topic[1]).map(topic => topic[0].id)){
                data.append('topicIds', id);
            }
        }
        if(file.length != 0){
            for( let f of file){
                data.append('files', { name: f.name, type: f.type, uri: f.uri });
            }
        }
        axios.post('/Question', data).then(res => nav.pop())
        .catch(err => console.log(err)); // TODO handle error
    }

    return (
        <Loader load={fetch}>
            <Text type='header' children={header}/>
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
            <Button onPress={selectFile}>SELECT FILES</Button>
            <Text children={file.map(f => f.name).join('\n')|| 'No file chosen'}/>
        </Loader>
    )
})
