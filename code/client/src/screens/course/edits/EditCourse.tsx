import React, { Screen, axios, useState } from '@/.';
import { View, Button, Loader, TextInput, ScrollView } from '@/components';

export default Screen('EditCourse', ({ params, nav }) => {
    const [name, setName] = useState('');
    const [number, setNumber] = useState('');
    const [changed, setChanged] = useState(false);

    const change = (set: (value: any) => void) => (value: any) => {
        setChanged(true);
        set(value);
    }

    const fetch = async () => {
        return axios.get('/Course/' + params.id).then(res => {
            setName(res.data.name);
            setNumber(res.data.number);
        })
    }

    const update = () => {
        axios.put('/Course/' + params.id, {
            name: name,
            number: number
        }).then(() => setChanged(false))
        .catch(err => {}); // TODO handle error
    }

    return (
        <Loader load={fetch}>
            <ScrollView>
                <TextInput label='name' defaultValue={name} onChangeText={change(setName)}/>
                <TextInput margin label='number' defaultValue={number} onChangeText={change(setNumber)}/>
                <Button margin children='Edit topics' onPress={() => nav.push('EditTopics', { courseId: params.id })}/>
                {/* <Button children='Edit channels' onPress={() => nav.push('EditTopics', { id: params.id })}/> */}
                {/* <Button children='Edit assitants' onPress={() => nav.push('EditTopics', { id: params.id })}/> */}
                <Button margin children='Update' onPress={update} disabled={!changed}/>
            </ScrollView>
        </Loader>
    );
});
