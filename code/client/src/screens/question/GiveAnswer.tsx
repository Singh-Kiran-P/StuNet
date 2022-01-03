import React, { Screen, useState, axios, update, show } from '@/.';
import { Text, View, Button, TextInput, CompactQuestion } from '@/components';

export default Screen('GiveAnswer', ({ nav, params: { question } }) => {
    let [error, setError] = useState('');
    let [title, setTitle] = useState('');
    let [body, setBody] = useState('');

    const submit = () => {
        setError('');
        axios.post('/Answer', {
            body: body,
            title: title,
            questionId: question.id
        }).then(
            () => {
                update('Question', { id: question.id });
                nav.pop();
            },
            show(setError)
        )
    }

    return (
        <View>
            <CompactQuestion question={question}/>
            <Text type='header' margin children='Give your answer'/>
            <TextInput margin label='Title' onChangeText={setTitle}/>
            <TextInput margin label='Body' multiline onChangeText={setBody}/>
            <Button margin icon='text-box-plus' children='Answer' disabled={!title || !body} toggled={error} onPress={submit}/>
            <Text type='error' margin hidden={!error} children={error}/>
        </View>
    )
})
