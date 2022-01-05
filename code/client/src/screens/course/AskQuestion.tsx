import React, { Screen, useState, axios, update, show } from '@/.';
import { Text, View, Button, TextInput, SelectTopics } from '@/components';

export default Screen('AskQuestion', ({ nav, params: { course, selected } }) => {
    let [actives, setActives] = useState(selected);
    let [error, setError] = useState('');
    let [title, setTitle] = useState('');
    let [body, setBody] = useState('');

    const submit = () => {
        setError('');
        axios.post('/Question', {
            body: body,
            title: title,
            topicIds: actives,
            courseId: course.id
        }).then(
            () => {
                update('Questions', { course: { id: course.id } });
                update('Profile', { email: '' });
                update('Home');
                nav.pop();
            },
            show(setError)
        )
    }

    return (
        <View>
            <Text type='header' children='Ask your question'/>
            <SelectTopics margin topics={course.topics} actives={actives} setActives={setActives}
                start={<Text type='hint' size='normal' margin='right' children='About'/>}
            />
            <TextInput margin label='Title' onChangeText={setTitle}/>
            <TextInput margin label='Body' multiline onChangeText={setBody}/>
            <Button margin icon='comment-plus' children='Ask' disabled={!title || !body} toggled={error} onPress={submit}/>
            <Text type='error' margin hidden={!error} children={error}/>
        </View>
    )
})
