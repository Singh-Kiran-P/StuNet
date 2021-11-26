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

    /**
     * Calculates the difference in milliseconds between the
     * given date and Date.now().
     *
     * @param date The datetime to be compared.
     * @returns the difference between the datetime and
     *          Date.now
     */
    function calculateNowDifference(date: Date): number
    {
        return calculateDateDifference(new Date(), date);
    }

    /**
     * Checks whether this.datetime is within the given
     * number of hours ago.
     *
     * https://stackoverflow.com/a/9224799
     *
     * @param time The time in milliseconds.
     * @returns true if this.datetime is whitin the given
     *          number of hours ago.
     */
    function isWithinTimeAgo(time: number): boolean
    {
        return calculateNowDifference(datetime) < time;
    }

    /**
     * Converts this.datetime to a jsx element.
     *
     * @returns the jsx element.
     */
    function renderDateTime(): JSX.Element
    {
        let output: string = datetime.toISOString();
        if (isWithinTimeAgo(HOUR)) {
            const passedTime: number = Math.floor(calculateNowDifference(datetime) / MINUTE);
            output = `${passedTime} minute${(passedTime > 1) ? 's' : ''} ago`;
        }
        else if (isWithinTimeAgo(DAY)) {
            const passedTime: number = Math.floor(calculateNowDifference(datetime) / HOUR);
            output = `${passedTime} hour${(passedTime > 1) ? 's' : ''} ago`;
        }
        return (
            <Text>{ output }</Text>
        );
    }

    return (
        <View>
            <Text>{title}</Text>
            <Text>{body}</Text>
            { renderDateTime() }
        </View>
    );
}
