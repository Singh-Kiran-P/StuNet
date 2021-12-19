import React, { Screen, axios, useState, Channel, Style, } from '@/.';
import { View, Button, Loader, Checkbox, ScrollView, TextInput, } from '@/components';
import { TouchableRipple } from 'react-native-paper';

type ChannelItem = {
    channel: Channel;
    checked: boolean;
}

const style: Style = Style.create({
    row: {
        flex: 1,
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'center'
    },
    rowItem: {
        flex: 1,
    },
    rowLongItem: {
        flex: 3,
    },
    rowButton: {
        padding: 2,
    },
});

export default Screen('EditChannels', ({ params, nav }) => {
    const [channelItems, setChannelItems] = useState<Array<ChannelItem>>([]);
    const [globalCheck, setGlobalCheck] = useState<boolean>(true);
    const [editableItem, setEditableItem] = useState<ChannelItem|null>(null);
    const [newChannelName, setNewChannelName] = useState<string>('');

    function init(data: Channel[]) {
        setChannelItems(data.map((channel: Channel) => ({ channel: channel, checked: false } as ChannelItem)));
    }

    async function fetch() {
        return axios.get('/Channel', { params: { courseId: params.courseId } }).then(res => init(res.data));
    }

    async function submit(name: string) {
        await axios.post('/Channel', { courseId: params.courseId, name: name })
            .catch(error => console.error(error)); // TODO: handle error
        fetch(); /* Thus fetch all to update full list, but will discard checks */
    }

    function update(channel: Channel) {
        axios.put('/Channel/' + channel.id, { name: channel.name })
            .then(() => setEditableItem(null))
            .catch(error => console.error(error)); // TODO: handle error
        resetGlobalChecked();
    }

    function resetGlobalChecked(check: boolean = globalCheck) {
        if (channelItems.every(item_ => item_.checked === check))
            setGlobalCheck(!check);
    }

    async function remove(channel: Channel) {
        await axios.delete('/Channel/' + channel.id)
            .then(() => channelItems.filter(item => item.channel.id !== channel.id))
            .catch(error => console.error(error)); // TODO: handle error
    }

    function flipChannelChecked(item: ChannelItem) {
        item.checked = !item.checked;
        setChannelItems(channelItems.slice());
        resetGlobalChecked();
    }

    function setAllChannelChecks(check: boolean) {
        channelItems.forEach(item => (item.checked = check));
        setGlobalCheck(!check);
    }

    function removeSelected() {
        for (const item of channelItems.filter(item => item.checked))
            remove(item.channel);
        fetch();
    }

    function renderRow(item: ChannelItem) {
        if (editableItem !== item) {
            return (
                <Checkbox.Item
                    mode='ios'
                    label={item.channel.name}
                    status={item.checked ? 'checked' : 'unchecked'}
                    // onPress={() => flipChannelCheck(i)}
                    />
            );
        }
        else {
            return (
                <View style={style.row}>
                    <TextInput
                        style={style.rowLongItem}
                        editable
                        // TODO: maxLength={?}
                        defaultValue={item.channel.name}
                        label='Name'
                        onChangeText={name => (item.channel.name = name)}
                        />
                    <Button
                        style={style.rowItem}
                        mode="contained"
                        onPress={() => update(item.channel)}
                        >
                        Update
                    </Button>
                </View>
            );
        }
    }

    return (
        <Loader load={fetch}>
            <ScrollView>
                <View style={style.row}>
                    <Button
                        style={style.rowItem}
                        onPress={() => setAllChannelChecks(globalCheck)}
                        >
                        { globalCheck ? 'Check all' : 'Uncheck all' }
                    </Button>
                    <Button
                        style={style.rowItem}
                        onPress={() => removeSelected()}
                        disabled={channelItems.every(item => item.checked === false)}
                        >
                        Remove selection
                    </Button>
                </View>
                <View>
                {
                    channelItems.map((item, i) =>
                        <TouchableRipple
                            key={i}
                            onPress={() => flipChannelChecked(item)}
                            onLongPress={() => setEditableItem(item)}
                            >
                            { renderRow(item) }
                        </TouchableRipple>
                    )
                }
                </View>
                <View style={style.row}>
                    <TextInput
                        style={style.rowLongItem}
                        editable
                        // TODO: maxLength={?}
                        label='Name'
                        onChangeText={name_ => setNewChannelName(name_)}
                        />
                    <Button
                        style={style.rowItem}
                        onPress={() => submit(newChannelName)}
                        >
                        Add channel
                    </Button>
                </View>
            </ScrollView>
        </Loader>
    );
});
