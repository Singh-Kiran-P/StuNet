import React, { Screen, useState, axios, update, show } from '@/.';
import { View, Text, Button, TextInput } from '@/components';
import { contains, merge } from '@/util/alg';

export default Screen('EditCourse', ({ params: { course }, nav }) => {
    let [description, setDescription] = useState(course.description || 'TODO description');
    let [number, setNumber] = useState(course.number);
    let [name, setName] = useState(course.name);
    let [error, setError] = useState('');

    let changed = !contains(course, { name, number }); // TODO add description

    const save = () => {
        axios.put('/Course/' + course.id, {
            name: name,
            number: number
        }).then(() => (update('Course', { id: course.id }), nav.pop()), show(setError))
    }

    return (
        <View flex>
            <TextInput label='Name' defaultValue={name} onChangeText={setName}/>
            <TextInput margin label='Number' defaultValue={number} onChangeText={setNumber}/>
            <TextInput margin label='Description' defaultValue={description} onChangeText={setDescription}/>
            <Button margin icon='content-save' children='Save' disabled={!name || !number || !changed} toggled={error} onPress={save}/>
            <Text type='error' margin hidden={!error} children={error}/>
            <Button align='bottom' icon='pencil' children='Edit topics' disabled={changed} onPress={() => nav.replace('EditTopics', { course })}/>
            <Button margin icon='pencil' children='Edit channels' disabled={changed} onPress={() => nav.replace('EditChannels', { course })}/>
            {/* TODO ? <Button children='Edit assitants' onPress={() => nav.replace('EditAssistants', { course })}/> */}
        </View>
    )
})
