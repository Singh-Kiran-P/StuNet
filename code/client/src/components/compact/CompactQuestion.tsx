import React, { extend, BaseQuestion, dateString, useNav } from '@/.';
import { View, Text, Chip, Icon, Touchable } from '@/components/base';

type Props = {
    question: BaseQuestion;
    selected?: number[];
}

export default extend<typeof Touchable, Props>(Touchable, ({ question, selected, ...props }) => {
    let nav = useNav();

    return (
        <Touchable type='row' padding='all-0.2' onPress={() => nav.navigate({ name: 'Question', params: { id: question.id }, merge: true })} {...props}>
            <Icon sizing='huge' coloring='accent' padding='vertical-0.2' name='comment-question'/>
            <View shrink grow margin='left'>
                <View type='header'>
                    <Text type='header' size='normal' children={question.title}/>
                    <Text type='hint' align='right' children={dateString(question.time)}/>
                </View>
                <Text numberOfLines={3} children={question.body}/>
                <View type='header' margin hidden={!question.topics.length} children={question.topics.map((topic, i) => (
                    <Chip margin='bottom,right-0.5' key={i} children={topic.name} active={!selected || selected.includes(topic.id)}/>
                ))}/>
            </View>
        </Touchable>
    )
})
