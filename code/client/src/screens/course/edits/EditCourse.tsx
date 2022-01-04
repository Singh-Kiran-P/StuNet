import React, { Screen, useState, axios, update, show } from '@/.';
import { View, Text, Button, TextInput } from '@/components';
import { contains } from '@/util/alg';

export default Screen('EditCourse', ({ nav, params: { course } }) => {
    let [name, setName] = useState(course.name);
    let [number, setNumber] = useState(course.number);
    let [email, setEmail] = useState(course.courseEmail);
    let [description, setDescription] = useState(course.description);
    let [error, setError] = useState('');

    let changed = !contains(course, { name, number, courseEmail: email, description });

    const save = () => {
        setError('');
        axios.put('/Course/' + course.id, {
            name: name,
            number: number,
            courseEmail: email,
            description: description
        }).then(
            () => {
                update('Course', { id: course.id });
                update('Profile', { email: '' });
                update('Courses');
                nav.pop();
            },
            show(setError)
        )
    }

    return (
        <View flex>
            <TextInput label='Name' defaultValue={name} onChangeText={setName}/>
            <TextInput margin label='Number' defaultValue={number} onChangeText={setNumber}/>
            <TextInput margin label='Email' defaultValue={email} onChangeText={setEmail}/>
            <TextInput margin label='Description' multiline defaultValue={description} onChangeText={setDescription}/>
            <Button margin='vertical' icon='content-save' children='Save' disabled={!name || !number || !changed} toggled={error} onPress={save}/>
            <Text type='error' margin hidden={!error} children={error}/>
            <Button align='bottom' icon='pencil' children='Edit topics' disabled={changed} onPress={() => nav.replace('EditTopics', { course })}/>
            <Button margin icon='pencil' children='Edit channels' disabled={changed} onPress={() => nav.replace('EditChannels', { course })}/>
        </View>
    )
})
