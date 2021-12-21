import React, { Screen, Answer, EmptyQuestion, QuestionSubscription, useState, useEffect, axios, dateString } from '@/.';
import { View, Text, Chip, List, Icon, Loader, Button, CompactAnswer } from '@/components';

export default Screen('Question', ({ nav, params: { id, subscribe } }) => {
    let [question, setQuestion] = useState(EmptyQuestion);
    let [answers, setAnswers] = useState<Answer[]>([]);
    let [notificationsEnabled, setNotifactionsEnabled] = useState<boolean>(true);

    const info = async () => {
        return axios.get('/Question/' + id).then(res => {
            setQuestion(res.data);
            nav.setParams({ course: res.data.course?.name || '' });
        })
    }

    const questions = async () => {
        return axios.get('/Answer/GetAnswersByQuestionId/' + id).then(res => {
            setAnswers(res.data);
        })
    }

    const infoNotification = async () => {
        axios.get('/QuestionSubscription/ByUserAndQuestionId/' + id)
            .then(response => setNotifactionsEnabled(response.data.length > 0))
            .catch(error => console.error(error));
    }
    const fetch = async () => Promise.all([info(), questions(), infoNotification()]);

    //TODO: Move this functionality to the server side
    function toggleNotificationSubcription(data: QuestionSubscription[]): void {
        if (data.length === 0) {
            axios.post('/QuestionSubscription/', { questionId: id } as QuestionSubscription)
            .then(() => setNotifactionsEnabled(!notificationsEnabled))
            .catch(error => console.error(error, notificationsEnabled));
        }
        else {
            axios.delete('/QuestionSubscription/' + data[0].id)
            .then(() => setNotifactionsEnabled(!notificationsEnabled))
            .catch(error => console.error(error, notificationsEnabled));
        }
        
    }

    useEffect(() => {
        if (subscribe === null) return;
        axios.get('/QuestionSubscription/ByUserAndQuestionId/' + id) // TODO test
            .then(response => toggleNotificationSubcription(response.data))
            .catch(error => console.error(error));
    }, [subscribe]);

    return (
        <Loader load={fetch}>
            <View type='header' hidden={!question.topics?.length} children={question.topics?.map((topic, i) => (
                <Chip margin='bottom,right-0.5' key={i} children={topic.name}/>
            ))}/>
            <View type='header'>
                <Text type='header' children={question.title}/>
                <Text type='hint' align='right' children={dateString(question.time)}/>
            </View>
            <List margin inner padding='bottom' ListHeaderComponent={
                <View>
                    <Text children={question.body}/>
                    <View type='row' margin>
                        <Icon sizing='large' margin='right-0.5' coloring='accent' name='download'/>
                        <Text type='link' {...{}/* TODO attachments */}>
                            Download 3 Attachments
                        </Text>
                    </View>
                    <Button margin='top-2' icon='text-box-plus' children='Give An Answer' onPress={() => nav.push('GiveAnswer', { question })}/>
                    <Text margin='top-2' type='header' children={answers.length ? 'Answers' : 'No answers yet'}/>
                </View>
            } data={answers} renderItem={answer => <CompactAnswer margin answer={answer.item}/>}/>
        </Loader>
    )
})
