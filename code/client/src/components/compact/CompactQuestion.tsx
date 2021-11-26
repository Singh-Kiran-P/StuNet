import React from '@/.';

import {
    View,
    Text
} from '@/components';

export type Question = {
    id: number;
    title: string;
    body: string;
    topicIds: number[];
}

type Props = {
	question: Question;
}

export default ({ question }: Props) => {
    return (
        <View>
            <Text>{question.id}</Text>
            <Text>{question.title}</Text>
            <Text>{question.body}</Text>
        </View>
    )
}
