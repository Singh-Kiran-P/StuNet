import React, { Screen, Answer, EmptyQuestion, useState, useEffect, axios, show, dateString } from '@/.';
import { View, Text, Chip, List, Icon, Loader, Button, CompactAnswer } from '@/components';

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
            setAnswers(res.data);
        })
    }

    const subscription = async () => {
        axios.get('/QuestionSubscription/ByUserAndQuestionId/' + id).then(res => {
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
            res => setSubscribed(res.data.id),
            show(setError)
        )
        else axios.delete('/QuestionSubscription/' + subscribed).then(
            () => setSubscribed(NaN),
            show(setError)
        )
    }, [subscribe, subscribed]);

    return (
        <Loader load={fetch}>
            <View pad='top'>
                <Text type='error' margin='bottom' hidden={!error} children={error}/>
                <View type='header' hidden={!question.topics?.length} children={question.topics?.map((topic, i) => (
                    <Chip margin='bottom,right-0.5' key={i} children={topic.name}/>
                ))}/>
                <View type='header'>
                    <Text type='header' children={question.title}/>
                    <Text type='hint' align='right' children={dateString(question.time)}/>
                </View>
            </View>
            <List margin inner padding='horizontal,bottom' ListHeaderComponent={
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
