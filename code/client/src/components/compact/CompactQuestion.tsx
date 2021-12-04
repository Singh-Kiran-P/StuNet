import React, { Style, useTheme, Question, dateString } from '@/.';
import { View, Text } from '@/components';

type Props = {
	question: Question;
}

const topics = ['Topic 1', 'Backtracking', 'Recursie']; // TODO load topics

export default ({ question }: Props) => {
    let [theme] = useTheme();

    const s = Style.create({
        question: {
            padding: theme.padding,
        },
    
        topic: { // TODO fix
            fontSize: theme.small,
            backgroundColor: theme.surface,
            borderRadius: 20,
            paddingVertical: 2,
            paddingHorizontal: 6,
            marginRight: 6
        }
    })

    return (
        <View style={s.question}>
            <View type='header'>
                <Text type='header' size='normal' children={question.title}/>
                <Text type='hint' alignRight children={dateString(question.time)}/>
            </View>
            <Text margin children={question.body}/>
            {topics.map((topic, i) => <Text key={i} style={s.topic} children={topic}/>)}
        </View>
    );
}
