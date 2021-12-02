import React, { axios, Screen, Style, Theme, useState, Answer } from '@/.';
import { Dimensions } from 'react-native';

import {
    Text,
    View,
    Loader,
    Button,
    ScrollView
} from '@/components';

export default Screen('Question', ({ params, nav }) => {
    let [answers, setAnswers] = useState<Answer[]>([]);
    let [course, setCourse] = useState('');
    let [author, setAuthor] = useState('');
    let [title, setTitle] = useState('');
    let [body, setBody] = useState('');
    let [date, setDate] = useState('');
    
    const s = Style.create({
        view: {
            flex: 1
        },

        screen: {
            height: 0,
            flexGrow: 1
        },

        body: {
            maxHeight: Dimensions.get('window').height / 2
        },

        button: {
            marginTop: Theme.margin
        }
    })

    const info = async () => {
        return axios.get('/Question/' + params.id).then(res => {
            let d = res?.data || {};
            setBody(d.body || '');
            setTitle(d.title || '');
            setCourse(d.course?.name || '');
            setAuthor(d.user || 'TODO USER');
            setDate(new Date(d.time).toDateString());
        })
    }

    const questions = async () => {
        return axios.get('/Answer/GetAnswersByQuestionId/' + params.id).then(res => {
            console.log(res.data);
            // setAnswers(res.data || []); // TODO
        })
    }

    const fetch = () => Promise.all([info(), questions()]);

    return (
        <Loader load={fetch} style={s.view}>
            <Text type='header' children={title}/>
            <Text>{author}</Text>
            <Text>{course}</Text>
            <Text>{date}</Text>
            <ScrollView style={s.screen}>
                <ScrollView style={s.body} nestedScrollEnabled>
                    <Text>{body}</Text>
                </ScrollView>
                <Text type='link' {...{}/* TODO icon, attachments */}>Download 3 Attachments</Text>
                <Button style={s.button} onPress={() => nav.push('CreateAnswer', {
                    questionId: params.id, question: title, date: date
                })} children='Answer'/>
                {answers.map(answer => (
                    <View>
                        <Text>{answer.title}</Text>
                        <Text>{answer.body}</Text>
                    </View>
                ))}
            </ScrollView>
        </Loader>
    )
})
