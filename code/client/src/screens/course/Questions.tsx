import React, { Screen, Question, useState, axios } from '@/.';
import { Text, List, Loader, Button, CompactQuestion, SelectTopics } from '@/components';

export default Screen('Questions', ({ params: { course }, nav }) => {
    let [questions, setQuestions] = useState<Question[]>([]);
    let [actives, setActives] = useState<number[]>([]);

    const display = (question: Question) => actives.every(i => question.topics.find(t => t.id === i));

    const fetch = async () => {
        return axios.get('/Course/' + course.id).then(res => { // TODO get questions only
            setQuestions(res.data.questions);
        })
    }

    return (
        <Loader load={fetch}>
            <SelectTopics margin topics={course.topics} actives={actives} setActives={setActives}/>
            <Button margin='top-2' icon='comment-plus' children='Ask a question' onPress={() => nav.push('AskQuestion', { course, selected: actives })}/>
            <Text type='hint' size='normal' margin='top-2' hidden={questions.filter(display).length} children='No questions match these topics'/>
            <List content padding='vertical' data={questions} renderItem={({ item, index }) => !display(item) ? null : (
                <CompactQuestion margin={!!index} question={item} selected={actives}/>
            )}/>
        </Loader>
    )
})
