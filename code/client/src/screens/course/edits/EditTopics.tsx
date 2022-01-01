import React, { Screen, BaseTopic, useState, axios, show, update } from '@/.';
import { View, Text, Button, Touchable, Checkbox, IconButton, SearchBar, TextInput } from '@/components';

export default Screen('EditTopics', ({ params: { course } }) => {
    let [topics, setTopics] = useState<[BaseTopic, boolean?][]>(course.topics.map(t => [t]));
    let [edit, setEdit] = useState<[number, string] | []>([]);
    let [error, setError] = useState('');

    let selection = topics.filter(t => t[1]);
    let save = () => update('Course', { id: course.id });

    const post = (name: string) => {
        axios.post('/Topic', {
            courseId: course.id,
            name: name
        }).then(
            res => (save(), setTopics(topics.concat([[{ id: res.data.id, name: name }]]))),
            show(setError)
        )
    }

    const put = (id: number, name: string) => {
        axios.put('/Topic/' + id, { name: name }).then(
            () => (save(), setTopics(topics.map(([t, s]) => [t.id === id ? { ...t, name } : t, s]))),
            show(setError)
        )
    }

    const remove = (ids: number[]) => {
        Promise.all(ids.map(id => axios.delete('/Topic/' + id))).then(
            () => (save(), setTopics(topics.filter(t => !ids.includes(t[0].id)))),
            show(setError)
        )
    }

    return (
        <View>
            <View type='row' margin='bottom-2'>
                <Button flex margin='right' icon='close' children='Clear' onPress={() => (setEdit([]), setTopics(topics.map(t => [t[0]])))}/>
                <Button flex margin='left' icon='delete' children='Delete' disabled={!selection.length || !!edit.length} toggled={topics}
                    onPress={() => (setError(''), setTopics(topics.map(t => [t[0], false])), remove(selection.map(t => t[0].id)))}
                />
            </View>
            {topics.map(([topic, selected], i) => {
                let editing = edit[0] === topic.id;
                let toggle = () => setTopics(topics.map(([t, s], j) => [t, i === j ? !s : s]));
                return (
                    <View type='header' key={i}>
                        <Touchable type='header' flex hidden={editing} onPress={toggle}>
                            <Checkbox checked={selected ? true : undefined}/>
                            <Text children={topic.name}/>
                        </Touchable>
                        <IconButton margin='left-1.5' icon='close' hidden={!editing} onPress={() => setEdit([])}/>
                        <TextInput flex mode='flat' hidden={!editing} defaultValue={topic.name} onChangeText={s => setEdit([edit[0] || 0, s])}/>
                        <IconButton margin='left' icon='pencil' hidden={editing} disabled={!!edit.length} onPress={() => setEdit([topic.id, topic.name])}/>
                        <IconButton margin='left' icon='content-save' hidden={!editing} disabled={!edit[1] || topic.name === edit[1]} onPress={() => {
                            if (edit.length) put(...edit);
                            setEdit([]);
                        }}/>
                    </View>
                )
            })}
            <SearchBar margin='top-2' icon='plus-thick' returnKeyType='done' placeholder='Add Topic' disableEmpty onSearch={(topic, set) => (set(''), post(topic))}/>
            <Text type='error' margin hidden={!error} children={error}/>
        </View>
    )
})
