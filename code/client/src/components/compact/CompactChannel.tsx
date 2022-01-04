import React, { extend, BaseChannel, useNav } from '@/.';
import { View, Text, Icon, Touchable } from '@/components/base';

type Props = {
    channel: BaseChannel;
}

export default extend<typeof Touchable, Props>(Touchable, ({ channel, ...props }) => {
    let nav = useNav();

    return (
        <Touchable type='row' padding='all-0.2' onPress={() => nav.navigate({ name: 'Channel', params: { id: channel.id }, merge: true })} {...props}>
            <Icon sizing='huge' coloring='accent' padding='vertical-0.2' name='chat'/>
            <View shrink grow margin='left'>
                <Text type='header' size='normal' children={channel.name}/>
                <Text type='hint' size='normal' hidden={channel.lastMessage} children='No messages yet'/>
                <Text numberOfLines={1} hidden={!channel.lastMessage} children={channel.lastMessage?.body}/>
            </View>
        </Touchable>
    )
})
