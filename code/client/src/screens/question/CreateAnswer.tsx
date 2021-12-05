import React, { Screen, useState, axios } from '@/.';
import { Text, View, Button, TextInput } from '@/components';

export default Screen('CreateAnswer', ({ params, nav }) => {
    let [title, setTitle] = useState('');
    let [body, setBody] = useState('');

    const submit = () => {
        axios.post('/Answer', {
            questionId: params.questionId,
            title: title,
            body: body
        }).then(() => nav.pop())
        .catch(err => {}); // TODO handle error
    }

    return (
        <View>
            <View type='header'>
                <Text type='header' children={params.question}/>
                <Text type='hint' align='right' children={params.date}/>
            </View>
            <TextInput margin label='Title' onChangeText={setTitle}/>
            <TextInput margin label='Body' multiline onChangeText={setBody}/>
            <Text margin>TODO files</Text>
            <Button margin children='Submit' onPress={submit}/>
        </View>
    )
})
