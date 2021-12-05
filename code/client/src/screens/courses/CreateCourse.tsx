import React, { Screen, useState, axios } from '@/.';
import { View, Button, TextInput } from '@/components';

export default Screen('CreateCourse', () => {
    const [name, setName] = useState('');
    const [number, setNumber] = useState('');
    const [description, setDescription] = useState(''); // TODO use description

    const submit = () => {
        axios.post('/Course', {
            name: name,
            number: number
        })
    }

    return (
        <View>
            <TextInput label='Name' onChangeText={setName}/>
            <TextInput margin label='Number' onChangeText={setNumber}/>
            <TextInput margin label='Description' multiline onChangeText={setDescription}/>
            <Button margin children='Create' disabled={!name || !number} onPress={submit}/>
        </View>
    )
})
