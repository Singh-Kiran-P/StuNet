/* eslint-disable curly */
import React from '@/.'; // 'react-native';
import {
    View,
    Text,
} from '@/components';
import { FlatList, StyleSheet } from 'react-native';

export type Question = {
    body: string;
    id: number;
    time: string;
    title: string;
    // topicIds: Array<number>;
}

type Props = {
	question: Question;
}

/* TODO: Move to whereever necessary */
const stylesheet = StyleSheet.create({
    question: {
        padding: 20,
    },
    header: {
        flexDirection: 'row',
        justifyContent: 'space-between',
    },
    title: {
        fontSize: 24,
    },
    body: {
        marginVertical: 10,
    },
    topic: {
        fontSize: 10, // 12,
        marginRight: 6, // 8,
        paddingVertical: 2, // 4,
        paddingHorizontal: 6, // 8,
        // marginVertical: 2,
        backgroundColor: 'lightgrey',
        borderRadius: 20.0,
    },
});


export function CompactQuestion(props: Props): JSX.Element
{
    const MAX_BODY_LENGTH: number = 100;

    const title: string = props.question.title;
    const body: string = props.question.body.substring(0, MAX_BODY_LENGTH) + ((props.question.body.length > 100) ? '...' : '');
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
        let output: string; // = datetime.toISOString();
        if (isWithinTimeAgo(HOUR)) {
            const passedTime: number = Math.floor(calculateNowDifference(datetime) / MINUTE);
            output = `${passedTime} minute${(passedTime !== 1) ? 's' : ''} ago`;
        }
        else if (isWithinTimeAgo(DAY)) {
            const passedTime: number = Math.floor(calculateNowDifference(datetime) / HOUR);
            output = `${passedTime} hour${(passedTime > 1) ? 's' : ''} ago`;
        }
        else {
            output = `${datetime.getDate()}/${datetime.getMonth()}/${datetime.getFullYear()}`;
        }
        return (
            <Text>{ output }</Text>
        );
    }

    return (
        <View style={stylesheet.question}>
            <View style={stylesheet.header}>
                <Text style={stylesheet.title}>{title}</Text>
                { renderDateTime() }
            </View>
            <Text style={stylesheet.body}>{body}</Text>
            <FlatList
                // https://reactnative.dev/docs/using-a-listview
                horizontal={true}
                data={[ // sample data
                    {key: 'Topic1'},
                    {key: 'Backtracking'},
                    {key: 'Recursie'},
                ]}
                renderItem={({item}) => <Text style={stylesheet.topic}>{item.key}</Text>} /* TODO: Use buttons instead(?) */
            />
        </View>
    );
}
