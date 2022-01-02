import React, { extend, BaseMessage, useNav, useTheme, Style, dateString, displayName, professor } from '@/.';
import { View, Text, Touchable } from '@/components/base';

type Props = {
	message: BaseMessage;
    sender: boolean;
    first?: boolean;
    last?: boolean;
}

export default extend<typeof View, Props>(View, ({ message, sender, first, last, ...props }) => {
    let [theme] = useTheme();
    let nav = useNav();

    let email = message.userMail;

    const s = Style.create({
        message: {
            maxWidth: '80%'
        },

        profile: {
            width: theme.massive,
            height: theme.massive
        },

        padding: {
            width: theme.massive
        },

        avatar: {
            color: theme.foreground,
            backgroundColor: theme.surface,
            textAlignVertical: 'center',
            textAlign: 'center',
            height: '100%',
            width: '100%'
        },

        body: {
            color: sender ? theme.bright : theme.foreground,
            backgroundColor: sender ? theme.primary : theme.surface
        },

        align: {
            alignSelf: sender ? 'flex-end' : 'flex-start'
        },

        prof: !professor(email) ? {} : {
            backgroundColor: theme.accent,
            color: theme.bright
        }
    })

	return (
        <View {...props} type='row' margin='bottom-0.5' style={[s.message, s.align]}>
            <Touchable margin='right,top' style={s.profile} radius='round' borderless hidden={sender || !first} onPress={() => {
                nav.navigate({ name: 'Profile', params: { email: email, logout: false }, merge: true })
            }}>
                <Text size='large' style={[s.avatar, s.prof]} children={email[0].toUpperCase()}/>
            </Touchable>
            <View hidden={sender || first} style={s.padding} margin='right'/>
            <View flex>
                <Text type='header' size='small' margin='bottom-0.2,top-0.5' style={s.align} hidden={sender || !first} children={displayName(email)}/>
                <Text radius padding='all-0.5' style={[s.body, s.align]} children={message.body}/>
                <Text type='hint' margin='top-0.2,bottom-0.5' style={s.align} hidden={!last} children={dateString(message.time)}/>
            </View>
        </View>
    )
})
