import React, {
    Screen,
    axios,
    useState,
    animate,
} from '@/.';
import {
    List,
    // Checkbox,
} from 'react-native-paper';
import {
    Text,
    LoadingWrapper,
    Button,
    CompactQuestion,
    Question,
} from '@/components';
// import { Text } from 'react-native-paper/lib/typescript/components/Avatar/Avatar';

export type Topic = {
    id: number;
    name: string;
}

export type Course = {
    id: number;
    name: string;
    number: string;
    topics: Array<Topic>;
    questions: Array<Question>;
}


export default Screen('Course', ({ params, nav }) => {
    const [name, setName] = useState('');
    const [number, setNumber] = useState('');
    const [topics, setTopics] = useState<Array<Topic>>([]);
    const [questions, setQuestions] = useState<Array<Question>>([]);

    const init = (result: { data: Course }) => {
        setName(result.data.name);
        setNumber(result.data.number);
        setTopics(result.data.topics);
        setQuestions(result.data.questions);
        console.log(result.data);
    };


    // On error: navigate to seperate error page? return to previous page and show error in snackbar?
    const fetch = async () => {
        const request: string = '/Course/' + params.id;
        return axios
            .get(request)
            .then(result => init(result))
            .catch(error => console.log(`request ${request} failed: ${error}`))
            ;
    };

    const renderTopicList = (): JSX.Element => {
        return (
            <List.Accordion title='Topics' onPress={animate}>
                {
                    topics.map((topic, i) =>
                        <Button
                            key={i}
                            onPress={() => nav.push('Course', params)} // TODO: show search results with this topic only
                            children={topic.name}
                        /> // {topic.name}</Button>
                    )
                }
            </List.Accordion>
        );
    };

    const renderQuestionList = (): JSX.Element => {
        return (
            <List.Accordion title='Questions' onPress={animate}>
                {
                    questions.map((question, i) =>
                        <CompactQuestion
                            key={i}
                            question={question}
                        />
                    )
                }
                <Button onPress={() => nav.push('Question', { id: 0 })} children='Question' />
            </List.Accordion>
        );
    };

    //TODO: edit title when screen is being called and name has been fetched
    return (
        <LoadingWrapper func={fetch}>
            {/* Text gives weird errors */}
            <Text>{name} ({number})</Text>
            {renderTopicList()}
            {renderQuestionList()}
            <Button onPress={() => nav.push('AskQuestion', { courseId: params.id })} children='Ask a question' />
        </LoadingWrapper>
    );
});
