import React, { axios, Screen, Style, useTheme, useState, Answer } from '@/.';
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
    let [title, setTitle] = useState('');
    let [body, setBody] = useState('');
    let [date, setDate] = useState('');
    let [theme] = useTheme();

    const s = Style.create({
        margin: {
            marginBottom: theme.margin
        },

        header: {
            flex: 0,
            flexDirection: 'row',
            justifyContent: 'space-between'
        },

        content: {
            flex: 1,
            flexGrow: 1
        },

        body: {
            maxHeight: Dimensions.get('window').height / 2,
            backgroundColor: theme.surface,
            borderRadius: theme.radius
        },

        bodyContent: {
            padding: theme.padding / 2
        }

    })

    const info = async () => {
        return axios.get('/Question/' + params.id).then(res => {
            let d = res?.data || {};
            setBody(d.body || '');
            setTitle(d.title || '');
            nav.setParams({ title: d.course?.name });
            setDate(new Date(d.time).toDateString());
        })
    }

    const questions = async () => {
        return axios.get('/Answer/GetAnswersByQuestionId/' + params.id).then(res => {
            console.log(res.data);
            // setAnswers(res.data || []); // TODO
        }).catch(err => console.log(err.response.data))
    }

    const fetch = () => Promise.all([info(), questions()]);

    return (
        <Loader load={fetch}>
            <View style={[s.header, s.margin]}>
                <Text type='header' children={title}/>
                <Text type='hint' children={date}/>
            </View>
            <ScrollView style={s.content}>
                <ScrollView style={[s.body, s.margin]} contentContainerStyle={s.bodyContent} nestedScrollEnabled>
                    <Text>{body}</Text>
                </ScrollView>
                <Text style={s.margin} type='link' {...{}/* TODO icon, attachments */}>Download 3 Attachments</Text>
                <Button onPress={() => nav.push('CreateAnswer', {
                    questionId: params.id, question: title, date: date
                })} children='Answer'/>
                {answers.map(answer => (
                    <View>
                        <Text children={answer.title}/>
                        <Text children={answer.body}/>
                    </View>
                ))}
            </ScrollView>
        </Loader>
    )
})
