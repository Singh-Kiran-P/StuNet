import React, { Screen, Answer, EmptyQuestion, useState, axios, dateString } from '@/.';
import { View, Text, Chip, List, Icon, Loader, Button, ScrollView, CompactAnswer } from '@/components';

export default Screen('Question', ({ nav, params: { id } }) => {
    let [question, setQuestion] = useState(EmptyQuestion);
    let [answers, setAnswers] = useState<Answer[]>([]);

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

    const fetch = async () => Promise.all([info(), questions()]);

    return (
        <Loader load={fetch}>
            <View type='header' hidden={!question.topics?.length} children={question.topics?.map((topic, i) => (
                <Chip margin='bottom,right-0.5' key={i} children={topic.name}/>
            ))}/>
            <View type='header'>
                <Text type='header' children={question.title}/>
                <Text type='hint' align='right' children={dateString(question.time)}/>
            </View>
            <List margin ListHeaderComponent={
                <ScrollView>
                    <Text children={question.body}/>
                    <View type='row' margin>
                        <Icon sizing='large' margin='right-0.5' coloring='accent' name='download'/>
                        <Text type='link' {...{}/* TODO attachments */}>
                            Download 3 Attachments
                        </Text>
                    </View>
                    <Button margin='top-2' icon='text-box-plus' children='Give An Answer' onPress={() => nav.push('GiveAnswer', { question })}/>
                    <Text margin='top-2' type='header' children={answers.length ? 'Answers' : 'No answers yet'}/>
                </ScrollView>
            } data={answers} renderItem={answer => <CompactAnswer margin answer={answer.item}/>}/>
        </Loader>
    )
})
