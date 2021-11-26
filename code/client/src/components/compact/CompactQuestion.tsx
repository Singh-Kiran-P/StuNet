import React from '@/.'; // 'react-native';
import {
    View,
    Text,
} from '@/components';

export type Question = {
    id: number;
    title: string;
    body: string;
    topicIds: Array<number>;
}

type Props = {
	question: Question;
}

export function CompactQuestion(props: Props): JSX.Element { // component
    return (
        <View>
            {/* <Text>{question.id}</Text> */}
            <Text>{props.question.title}</Text>
            <Text>{props.question.body}</Text>
            {/* <Text>{question.id}</Text> */}
        </View>
    );
}
