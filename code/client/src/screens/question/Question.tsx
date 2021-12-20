import React, { axios, Screen, Style, useTheme, useState, Answer, dateString, QuestionSubscription } from '@/.';
import { Text, View, Loader, Button, ScrollView } from '@/components';
import { Dimensions } from 'react-native';

export default Screen('Question', ({ params, nav }) => {
    let [answers, setAnswers] = useState<Answer[]>([]);
    let [title, setTitle] = useState('');
    let [body, setBody] = useState('');
    let [date, setDate] = useState('');
    let [theme] = useTheme();
    const [notificationsEnabled, setNotifactionsEnabled] = useState<boolean>(true);

    const s = Style.create({
        content: {
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

    const infoNotification = async () => {
        axios.get('/QuestionSubscription/ByUserAndQuestionId/' + params.id)
            .then(response => setNotifactionsEnabled(response.data.length > 0))
            .catch(error => console.error(error));
    }
    const fetch = async () => Promise.all([info(), questions(), infoNotification()]);

    //TODO: Move this functionality to the server side
    function toggleNotificationSubcription(data: QuestionSubscription[]): void {
        if (data.length === 0) {
            axios.post('/QuestionSubscription/', { questionId: params.id } as QuestionSubscription)
            .then(setNotifactionsEnabled(!notificationsEnabled))
            .catch(error => console.error(error, notificationsEnabled));
        }
        else {
            axios.delete('/QuestionSubscription/' + data[0].id)
            .then(setNotifactionsEnabled(!notificationsEnabled))
            .catch(error => console.error(error, notificationsEnabled));
        }
        
    }

    /**
     * Updates the notification on the server, and if succes
     * updates the local notification.
     */
    function updateNotificationSubscription(): void {
        axios.get('/QuestionSubscription/ByUserAndQuestionId/' + params.id)
            .then(response => toggleNotificationSubcription(response.data))
            .catch(error => console.error(error));
    }

    return (
        <Loader load={fetch}>
            <View type='header'>
                <Text type='header' children={title}/>
                {/* Temporary button which should be moved to the page header as an icon */}
                <Button margin align='right' children={(notificationsEnabled ? 'Disable' : 'Enable') + ' notifications'} onPress={() => updateNotificationSubscription()}/>
                <Text type='hint' align='right' children={date}/>
            </View>
            <ScrollView margin style={s.content} flex>
                <ScrollView style={s.body} contentContainerStyle={s.bodyContent} nestedScrollEnabled>
                    <Text>{body}</Text>
                </ScrollView>
                <Text margin type='link' {...{}/* TODO icon, attachments */}>Download 3 Attachments</Text>
                <Button margin children='Answer' onPress={() => nav.push('CreateAnswer', {
                    questionId: params.id, question: title, date: date, course: params.course
                })}/>
                {answers.map((answer, i) => ( // TODO lazy list view
                    <View key={i} style={s.answer} onTouchEnd={() => nav.push('Answer', { ...answer, course: params.course || '' })}>
                        <View type='header'>
                            <Text type='header' size='normal' children={answer.title}/>
                            <Text type='hint' align='right' children={dateString(answer.dateTime)}/>
                        </View>
                        <Text numberOfLines={1} ellipsizeMode='tail' children={answer.body}/>
                    </View>
                ))}
            </ScrollView>
        </Loader>
    )
})
