import React, {
    Screen,
    axios,
    useState,
    animate,
} from '@/.';
import {
    List,
    Text,
    // Checkbox,
} from 'react-native-paper';
import {
    LoadingWrapper,
    Button,
} from '@/components';
// import { Text } from 'react-native-paper/lib/typescript/components/Avatar/Avatar';

type Topic = {
    id: number;
    name: string;
}

type Course = {
    id: number;
    topics: Array<Topic>,

}

export default Screen('Course', ({ params, nav }) => {
    const [topics, setTopics] = useState<Array<Topic>>([]);


    const init = (result: {data: Course}) => {
        setTopics(result.data.topics);
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

    return (
        <LoadingWrapper func={fetch}>
            { renderTopicList() }
        </LoadingWrapper>
    );
});
