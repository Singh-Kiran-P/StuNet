import React, { extend, BaseMessage, Style, useTheme, dateString } from '@/.';
import { View, Text } from '@/components/base';

type Props = {
	message: BaseMessage;
    sender: boolean;
}

export default extend<typeof View, Props>(View, ({ message, sender, ...props }) => {
    let [theme] = useTheme();

    const s = Style.create({
        body: {
            color: sender ? theme.bright : theme.foreground,
            backgroundColor: sender ? theme.primary : theme.surface,
            maxWidth: '80%'
        },

        align: {
            alignSelf: sender ? 'flex-end' : 'flex-start'
        }
    })

	return (
        <View {...props}>
            <Text size='small' style={s.align} children={message.userMail}/>
            <Text radius padding='all-0.5' style={[s.body, s.align]} children={message.body}/>
            <Text type='hint' style={s.align} children={dateString(message.time)}/>
        </View>
    )
})
