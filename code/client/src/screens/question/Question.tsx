import React, { Screen, Answer, EmptyQuestion, useState, useEffect, axios, update, show, dateString, timeSort } from '@/.';
import { View, Text, Chip, List, Loader, Button, CompactAnswer } from '@/components';

export default Screen('Question', ({ nav, params: { id, subscribe } }) => {
    let [subscribed, setSubscribed] = useState<null | number>(null);
    let [question, setQuestion] = useState(EmptyQuestion);
    let [answers, setAnswers] = useState<Answer[]>([]);
    let [error, setError] = useState('');

    const info = async () => {
        return axios.get('/Question/' + id).then(res => {
            setQuestion(res.data);
            nav.setParams({ course: res.data.course?.name || '' });
        })
    }

    const questions = async () => {
        return axios.get('/Answer/GetAnswersByQuestionId/' + id).then(res => {
            setAnswers(timeSort(res.data));
        })
    }

    const subscription = async () => {
        return axios.get('/QuestionSubscription/ByUserAndQuestionId/' + id).then(res => {
            setSubscribed(res.data.length ? res.data[0].id : NaN);
            nav.setParams({ subscribe: res.data.length > 0 });
        })
    }

    const fetch = async () => Promise.all([info(), questions(), subscription()]);

    useEffect(() => {
        if (subscribe === null) return;
        if (subscribed === null) return;
        if (subscribe === !isNaN(subscribed)) return;
        if (subscribe) axios.post('/QuestionSubscription/', { questionId: id }).then(
            res => (setSubscribed(res.data.id), update('Home')),
            show(setError)
        )
        else axios.delete('/QuestionSubscription/' + subscribed).then(
            () => (setSubscribed(NaN), update('Home')),
            show(setError)
        )
    }, [subscribe, subscribed]);

    return (
        <Loader load={fetch}>
            <View pad='top'>
                <Text type='error' margin='bottom' hidden={!error} children={error}/>
                <View type='header' hidden={!question.topics?.length} children={question.topics?.map((topic, i) => (
                    <Chip margin='bottom,right-0.5' key={i} children={topic.name} active/>
                ))}/>
                <View type='header'>
                    <Text type='header' children={question.title}/>
                    <Text type='hint' align='right' children={dateString(question.time)}/>
                </View>
            </View>
            <List margin inner padding='horizontal,bottom' ListHeaderComponent={
                <View>
                    <Text children={question.body}/>
                    <Button margin='top-2' icon='text-box-plus' children='Give An Answer' onPress={() => nav.push('GiveAnswer', { question })}/>
                    <Text type='hint' size='normal' margin='top-2' hidden={answers.length} children='No answers have been given'/>
                    <Text type='header' color='placeholder' margin='top-2' hidden={!answers.length} children='Answers'/>
                </View>
            } data={answers} renderItem={answer => <CompactAnswer margin={!!answer.index} answer={answer.item}/>}/>
        </Loader>
    )
})
