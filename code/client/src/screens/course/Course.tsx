import React, {
    Screen,
    axios,
    useState,
    Topic,
    Course,
} from '@/.';
import {
    Text,
    Button,
    Loader,
    Question,
    Collapse,
    CompactQuestion,
    ScrollView,
} from '@/components';

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
                <Text>{name} ({number})</Text>
                <Collapse title='Topics'>
                    {topics.map((topic, i) => ( // TODO: show search results with this topic only on click
                        <Button key={i}
                            onPress={() => nav.push('Course', params)}
                            children={topic.name}
                            />
                    ))}
                </Collapse>
                <Collapse title='Questions'>
                    {questions.map((question, i) => (
                        <CompactQuestion key={i} question={question}/>
                    ))}
                </Collapse>
                <Button onPress={() => nav.push('AskQuestion', { courseId: params.id })} children='Ask a question' />
                <Button onPress={() => nav.push('EditCourse', { id: params.id })} children='Edit course' />
            </ScrollView>
        </Loader>
    );
});
