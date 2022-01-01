import React, { Screen, Question, useState, useEffect, axios, show, timeSort } from '@/.';
import { View, Text, List, Button, CompactQuestion, SelectTopics } from '@/components';

export default Screen('Questions', ({ nav, params: { course, search, update } }) => {
    let [questions, setQuestions] = useState<Question[]>([]);
    let [actives, setActives] = useState<number[]>([]);
    let [refresh, setRefresh] = useState(true);
    let [error, setError] = useState('');

    const display = (question: Question) => actives.every(i => question.topics.find(t => t.id === i));

    useEffect(() => {
        if (!refresh) setRefresh(true);
        axios.get('/Question/GetQuestionsByCourseId/search/' + course.id, { params: { name: search }}).then(
            res => (setRefresh(false), setQuestions(timeSort(res.data))),
            show(setError)
        )
    }, [search, update]);

    return (
        <View flex>
            <SelectTopics pad='top' margin topics={course.topics} actives={actives} setActives={setActives}/>
            <Button pad='top' icon='comment-plus' children='Ask a question'
                onPress={() => nav.push('AskQuestion', { course, selected: actives })}
            />
            <Text type='error' pad='top' hidden={!error} children={error}/>
            <Text type='hint' size='normal' pad='top' hidden={questions.filter(display).length} children='No questions match these topics'/>
            <List inner padding data={questions} refreshing={refresh} renderItem={question => !display(question.item) ? null : (
                <CompactQuestion margin={!!question.index} question={question.item} selected={actives}/>
            )}/>
        </View>
    )
})
