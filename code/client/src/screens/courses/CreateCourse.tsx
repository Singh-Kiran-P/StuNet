import React, { Screen, useState, axios, update, show } from '@/.';
import { View, Text, Button, TextInput } from '@/components';

export default Screen('CreateCourse', ({ nav }) => {
    let [name, setName] = useState('');
    let [number, setNumber] = useState('');
    let [email, setEmail] = useState('');
    let [description, setDescription] = useState('');
    let [error, setError] = useState('');

    const submit = () => {
        axios.post('/Course', {
            name: name,
            number: number,
            courseEmail: email,
            description: description
        }).then(res => (update('Courses'), nav.replace('Course', { id: res.data.id })), show(setError))
    }

    return (
        <View>
            <TextInput label='Name' onChangeText={setName}/>
            <TextInput margin label='Number' onChangeText={setNumber}/>
            <TextInput margin label='Email' onChangeText={setEmail}/>
            <TextInput margin label='Description' multiline onChangeText={setDescription}/>
            <Button margin icon='book-plus' children='Create' disabled={!name || !number} toggled={error} onPress={submit}/>
            <Text type='error' margin hidden={!error} children={error}/>
        </View>
    )
})
