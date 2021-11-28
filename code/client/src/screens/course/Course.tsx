import React, { Screen, axios, useState } from '@/.';

import {
    Text,
    Button,
    Loader,
    Question,
    Collapse,
    CompactQuestion
} from '@/components';

type Topic = {
    id: number;
    name: string;
}

type Course = {
    id: number;
    name: string;
    number: string;
    topics: Topic[];
    questions: Question[];
}

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
    }

    const fetch = async () => axios.get('/Course/' + params.id).then(res => init(res.data));

    //TODO: edit title when screen is being called and name has been fetched
    return (
        <Loader load={fetch}>
            {/* Text gives weird errors */}
            <Text>{name} ({number})</Text>
            <Collapse title='Topics'>
                {topics.map((topic, i) => ( // TODO: show search results with this topic only
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
        </Loader>
    );
});
