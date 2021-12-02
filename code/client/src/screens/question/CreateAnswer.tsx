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
        axios.post('/Answer', {
            questionId: params.questionId,
            title: title,
            body: body
        }).then(() => nav.navigate('Question', { id: params.questionId })).catch(err => console.log(err.response.data));
    }

    return (
        <View>
            <Text style={s.margin}>{params.question}</Text>
            <Text style={s.margin}>{params.date}</Text>
            <TextInput style={s.margin} label='Title' onChangeText={setTitle}/>
            <TextInput style={s.margin} label='Body' multiline onChangeText={setBody}/>
            <Text style={s.margin}>TODO files</Text>
            <Button children='Submit' onPress={submit}/>
        </View>
    )
})
