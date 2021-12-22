import React, { extend, BaseAnswer, dateString, useNav, useTheme } from '@/.';
import { View, Text, Icon, Touchable } from '@/components/base';

type Props = {
    answer: BaseAnswer;
}

export default extend<typeof Touchable, Props>(Touchable, ({ answer, ...props }) => {
    let nav = useNav();
    let theme = useTheme()[0];

    return ( // TODO push?
        <Touchable type='row' padding='all-0.2' onPress={() => nav.navigate({ name: 'Answer', params: { id: answer.id }, merge: true })} {...props}>
            <Icon sizing='huge' padding='vertical-0.2' name={answer.isAccepted ? 'check' : 'text-box'} style={{color: answer.isAccepted ? 'green' : theme.placeholder}} />
            <View shrink grow margin='left'>
                <View type='header'>
                    <Text type='header' size='normal' children={answer.title}/>
                    <Text type='hint' align='right' children={dateString(answer.time)}/>
                </View>
                <Text numberOfLines={3} children={answer.body}/>
            </View>
        </Touchable>
    )
})
