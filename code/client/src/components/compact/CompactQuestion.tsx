import React from '@/.'; // 'react-native';
import {
    View,
    Text,
} from '@/components';

export type Question = {
    id: number;
    title: string;
    body: string;
    time: string;
    // topicIds: Array<number>;
}

type Props = {
	question: Question;
}

export function CompactQuestion(props: Props): JSX.Element { // component
    const title: string = props.question.title;
    const body: string = props.question.body;
    const datetime: Date = new Date(props.question.time);

    return (
        <View>
            <Text>{title}</Text>
            <Text>{body}</Text>
            <Text>{datetime.toISOString()}</Text>
        </View>
    );
}
