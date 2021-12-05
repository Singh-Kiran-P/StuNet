import React, { Screen, axios, useState, Topic, Course, Question } from '@/.';
import { Button, Loader, Collapse, ScrollView, CompactQuestion } from '@/components';

export default Screen('Course', ({ params, nav }) => {
    const [name, setName] = useState('');
    const [number, setNumber] = useState('');
    const [topics, setTopics] = useState<Topic[]>([]);
    const [questions, setQuestions] = useState<Question[]>([]);

    const init = (data: Course) => {
        setName(data.name);
        setNumber(data.number);
        setTopics(data.topics);
        setQuestions(data.questions);
        nav.setParams({ name: data.name });
    }

    const fetch = async () => {
        return axios.get('/Course/' + params.id)
            .then(res => init(res.data))
            .catch(err => {}) // TODO handle error
    }

    return (
        <Loader load={fetch}>
            <ScrollView>
                <Collapse title='Topics'>
                    {topics.map((topic, i) => ( // TODO: show search results with this topic only on click
                        <Button key={i}
                            onPress={() => nav.push('Course', params)}
                            children={topic.name}
                        />
                    ))}
                </Collapse>
                <Collapse margin title='Questions'>
                    {questions.map((question, i) => (
                        <CompactQuestion key={i} question={question}/>
                    ))}
                </Collapse>
                <Button margin children='Ask a question' onPress={() => nav.push('AskQuestion', { courseId: params.id })}/>
                <Button margin children='Edit course' onPress={() => nav.push('EditCourse', { id: params.id })}/>
            </ScrollView>
        </Loader>
    );
});
