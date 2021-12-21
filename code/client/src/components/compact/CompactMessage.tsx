import React, { extend, BaseMessage, Style, useTheme, dateString } from '@/.';
import { View, Text } from '@/components/base';

type Props = {
	message: BaseMessage;
    sender: boolean;
}

const prof = (email: string) => !email.endsWith('@student.uhasselt.be');
const name = (email: string) => {
    let name = email.slice(0, (i => i < 0 ? undefined : i)(email.lastIndexOf('@')));
    return name.replace('.', ' ').split(' ').map(s => {
        return s[0].toUpperCase() + s.slice(1).toLowerCase();
    }).join(' ');
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
        },

        prof: !prof(message.userMail) ? {} : {
            backgroundColor: theme.accent,
            color: theme.bright
        }
    })

	return (
        <View {...props}>
            <Text size='small' style={s.align} hidden={sender} children={name(message.userMail)}/>
            <Text radius padding='all-0.5' margin='vertical-0.2' style={[s.body, s.align, s.prof]} children={message.body}/>
            <Text type='hint' margin='bottom-0.5' style={s.align} children={dateString(message.time)}/>
        </View>
    )
})
