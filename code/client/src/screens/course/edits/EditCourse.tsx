import React, { Screen, useState, axios, update, show } from '@/.';
import { View, Text, Button, TextInput } from '@/components';

export default Screen('EditCourse', ({ params: { course }, nav }) => {
    let [description, setDescription] = useState(course.description || 'TODO description');
    let [number, setNumber] = useState(course.number);
    let [name, setName] = useState(course.name);
    let [error, setError] = useState('');

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
            <Button margin icon='content-save' children='Save' disabled={!name || !number} toggled={error} onPress={save}/>
            <Text type='error' margin hidden={!error} children={error}/>
            <Button align='bottom' icon='pencil' children='Edit topics' onPress={() => nav.push('EditTopics', { course })}/>
            {/* <Button children='Edit channels' onPress={() => nav.push('EditTopics', { course })}/> */}
            {/* <Button children='Edit assitants' onPress={() => nav.push('EditTopics', { course })}/> */}
        </View>
    )
})
