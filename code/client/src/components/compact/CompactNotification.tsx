import React, { extend, BaseQuestion, BaseAnswer } from '@/.';
import CompactQuestion from '@/components/compact/CompactQuestion';
import CompactAnswer from '@/components/compact/CompactAnswer';
import { View, Text, Touchable } from '@/components/base';

type Props = {
    question?: BaseQuestion;
    answer?: BaseAnswer;
}

export default extend<typeof Touchable, Props>(Touchable, ({ question, answer, ...props }) => {
    return (
        <View {...props}>
            <Text type='hint' size='normal' hidden={!question} children='A question has been asked'/>
            <Text type='hint' size='normal' hidden={!answer} children='An answer has been given'/>
            {question && <CompactQuestion question={question}/>}
            {answer && <CompactAnswer answer={answer}/>}
        </View>
    )
})
