import React, { axios, Screen, Style, useTheme, useState, Answer, dateString } from '@/.';
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
            flexDirection: 'row',
            alignItems: 'center',
            flexWrap: 'wrap'
        },

        right: {
            marginLeft: 'auto'
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
        },

        answer: {
            padding: theme.padding / 2,
            backgroundColor: theme.surface,
            borderRadius: theme.radius,
            marginTop: theme.margin
        }

    })

    const info = async () => {
        return axios.get('/Question/' + params.id).then(res => {
            let d = res?.data || {};
            setBody(d.body || '');
            setTitle(d.title || '');
            setDate(dateString(d.time));
            nav.setParams({ course: d.course?.name });
        })
    }

    const questions = async () => {
        return axios.get('/Answer/GetAnswersByQuestionId/' + params.id).then(res => {
            setAnswers(Array.isArray(res.data) ? res.data : []);
        })
    }

    const fetch = () => Promise.all([info(), questions()]);

    return (
        <Loader load={fetch}>
            <View style={[s.header, s.margin]}>
                <Text type='header' children={title}/>
                <Text type='hint' style={s.right} children={date}/>
            </View>
            <ScrollView style={s.content}>
                <ScrollView style={[s.body, s.margin]} contentContainerStyle={s.bodyContent} nestedScrollEnabled>
                    <Text>{body}</Text>
                </ScrollView>
                <Text style={s.margin} type='link' {...{}/* TODO icon, attachments */}>Download 3 Attachments</Text>
                <Button onPress={() => nav.push('CreateAnswer', {
                    questionId: params.id, question: title, date: date
                })} children='Answer'/>
                {answers.map((answer, i) => (
                    <View key={i} style={s.answer} onTouchEnd={() => nav.push('Answer', { ...answer, course: params.course || '' })}>
                        <View style={s.header}>
                            <Text type='header' size='medium' children={answer.title}/>
                            <Text type='hint' style={s.right} children={dateString(answer.dateTime)}/>
                        </View>
                        <Text numberOfLines={1} ellipsizeMode='tail' children={answer.body}/>
                    </View>
                ))}
            </ScrollView>
        </Loader>
    )
})
