import React, { Screen, useState, axios, update, show } from '@/.';
import { View, Text, Button, TextInput } from '@/components';

export default Screen('CreateCourse', ({ nav }) => {
    let [name, setName] = useState('');
    let [number, setNumber] = useState('');
    let [description, setDescription] = useState('');
    let [email, setEmail] = useState('');
    let [error, setError] = useState('');

    const submit = () => {
        setError('');
        axios.post('/Course', {
            name: name,
            email: email,
            number: number,
            courseEmail: email,
            description: description
        }).then(
            res => {
                update('Courses');
                update('Profile', { email: '' });
                nav.replace('Course', { id: res.data.id });
            },
            show(setError)
        )
    }

    return (
        <View>
            <TextInput label='Name' onChangeText={setName}/>
            <TextInput margin label='Number' onChangeText={setNumber}/>
            <TextInput margin label='Email' onChangeText={setEmail}/>
            <TextInput margin label='Description' multiline onChangeText={setDescription}/>
            <Button margin icon='book-plus' children='Create' disabled={!name || !number || !email} toggled={error} onPress={submit}/>
            <Text type='error' margin hidden={!error} children={error}/>
        </View>
    )
})
