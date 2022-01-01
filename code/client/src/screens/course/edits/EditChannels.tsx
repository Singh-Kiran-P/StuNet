import React, { Screen, BaseChannel, useState, axios, show, update } from '@/.';
import { View, Text, Button, Touchable, Checkbox, IconButton, SearchBar, TextInput } from '@/components';

export default Screen('EditChannels', ({ params: { course } }) => {
    let [channels, setChannels] = useState<[BaseChannel, boolean?][]>(course.channels.map(t => [t]));
    let [edit, setEdit] = useState<[number, string] | []>([]);
    let [error, setError] = useState('');

    let selection = channels.filter(t => t[1]);
    let save = () => update('Course', { id: course.id });

    const post = (name: string) => {
        axios.post('/Channel', {
            courseId: course.id,
            name: name
        }).then(
            res => (save(), setChannels(channels.concat([[{ id: res.data.id, name: name }]]))),
            show(setError)
        )
    }

    const put = (id: number, name: string) => {
        axios.put('/Channel/' + id, { name: name }).then(
            () => (save(), setChannels(channels.map(([t, s]) => [t.id === id ? { ...t, name } : t, s]))),
            show(setError)
        )
    }

    const remove = (ids: number[]) => {
        Promise.all(ids.map(id => axios.delete('/Channel/' + id))).then(
            () => (save(), setChannels(channels.filter(t => !ids.includes(t[0].id)))),
            show(setError)
        )
    }

    return (
        <View>
            <View type='row' margin='bottom-2'>
                <Button flex margin='right' icon='close' children='Clear' onPress={() => (setEdit([]), setChannels(channels.map(t => [t[0]])))}/>
                <Button flex margin='left' icon='delete' children='Delete' disabled={!selection.length || !!edit.length} toggled={channels}
                    onPress={() => (setError(''), setChannels(channels.map(t => [t[0], false])), remove(selection.map(t => t[0].id)))}
                />
            </View>
            {channels.map(([channel, selected], i) => {
                let editing = edit[0] === channel.id;
                let toggle = () => setChannels(channels.map(([t, s], j) => [t, i === j ? !s : s]));
                return (
                    <View type='header' key={i}>
                        <Touchable type='header' flex hidden={editing} onPress={toggle}>
                            <Checkbox checked={selected ? true : undefined}/>
                            <Text children={channel.name}/>
                        </Touchable>
                        <IconButton margin='left-1.5' icon='close' hidden={!editing} onPress={() => setEdit([])}/>
                        <TextInput flex mode='flat' hidden={!editing} defaultValue={channel.name} onChangeText={s => setEdit([edit[0] || 0, s])}/>
                        <IconButton margin='left' icon='pencil' hidden={editing} disabled={!!edit.length} onPress={() => setEdit([channel.id, channel.name])}/>
                        <IconButton margin='left' icon='content-save' hidden={!editing} disabled={!edit[1] || channel.name === edit[1]} onPress={() => {
                            if (edit.length) put(...edit);
                            setEdit([]);
                        }}/>
                    </View>
                )
            })}
            <SearchBar margin='top-2' icon='plus-thick' returnKeyType='done' placeholder='Add Channel' disableEmpty onSearch={(channel, set) => (set(''), post(channel))}/>
            <Text type='error' margin hidden={!error} children={error}/>
        </View>
    )
})
