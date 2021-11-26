/* eslint-disable curly */
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

export function CompactQuestion(props: Props): JSX.Element
{
    const title: string = props.question.title;
    const body: string = props.question.body;
    const datetime: Date = new Date(props.question.time);

    /**
     * An hour in milliseconds.
     */
    const SECOND: number = 1000;
    const MINUTE: number = 60 * SECOND;
    const HOUR: number = 60 * MINUTE;
    const DAY: number = 24 * HOUR;

    /**
     * Calculates the difference in milleseconds between a
     * and b.
     *
     * NOTE: Date.now() is behind on the winter time (on my
     * computer at least).
     *
     * @param b The time that will be substracted.
     * @param a The time that will substract.
     * @returns the difference in milliseconds.
     */
    function calculateDateDifference(b: Date, a: Date): number
    {
        /* TODO: take the winter time into account dynamically. */
        return b.getTime() + HOUR - a.getTime()
    }

    return (
        <View>
            <Text>{title}</Text>
            <Text>{body}</Text>
            <Text>{datetime.toISOString()}</Text>
        </View>
    );
}
