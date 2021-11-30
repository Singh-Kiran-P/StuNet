import React, { Screen, Style, Theme, useState, axios } from '@/.';

import {
    Text,
    View,
    Button,
    TextInput
} from '@/components';

export default Screen('CreateAnswer', ({ params, nav }) => {
    let [title, setTitle] = useState('');
    let [body, setBody] = useState('');

    const s = Style.create({
        margin: {
            marginBottom: Theme.margin
        }
    })

    const submit = () => {
        axios.post('/Answer');
    }

    return (
        <View>
            <Text style={s.margin}>TODO information</Text>
            <TextInput style={s.margin} label='Title' onChangeText={setTitle}/>
            <TextInput style={s.margin} label='Body' multiline onChangeText={setBody}/>
            <Text style={s.margin}>TODO files</Text>
            <Button children='Submit' onPress={submit}/>
        </View>
    )
})
