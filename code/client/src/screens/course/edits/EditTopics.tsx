/* eslint-disable curly */
/* eslint-disable jsx-quotes */
import React, {
    Screen,
    axios,
    useState,
    // AxiosResponse, // TODO: reexport from 'axios'
    Course,
    Topic,
    Style,
} from '@/.';
import {
    View,
    Button,
    Pressable,
    Loader,
    Checkbox,
    ScrollView,
    TextInput,
} from '@/components';

type TopicItem = {
    topic: Topic;
    checked: boolean;
}

const style: Style = Style.create({
    row: {
        flex: 1,
        flexDirection: 'row',
        justifyContent:'space-between',
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

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export default Screen('EditTopics', ({ params, nav }) => {
    const [topicItems, setTopicItems] = useState<Array<TopicItem>>([]);
    const [globalCheck, setGlobalCheck] = useState<boolean>(true);
    const [editableItem, setEditableItem] = useState<TopicItem|null>(null);
    const [newTopicName, setNewTopicName] = useState<string>('');

    function init(data: Course): void
    {
        setTopicItems(data.topics.map(topic => ({ topic: topic, checked: false } as TopicItem)));
    }

    async function fetch() //: Promise<void | AxiosResponse<any, any>>
    {
        return axios.get('/Course/' + params.courseId).then(res => init(res.data));
    }

    /**
     * Submits the topic name and awaits an ok result.
     * @param name The topic name.
     */
    async function submit(name: string): Promise<void>
    {
        await axios.post('/Topic', { courseId: params.courseId, name: name })
            // .then(() => setTopics(topicItems + [])) /* The id is required */
            .catch(error => console.error(error)); // TODO: handle error
        fetch(); /* Thus fetch all to update full list, but will discard checks */
    }

    /**
     * Updates the topic in the server.
     *
     * @param topic The topic to be updated.
     */
    function update(topic: Topic): void
    {
        axios.put('/Topic/' + topic.id, { name: topic.name })
            .then(() => setEditableItem(null)) /* Topic name will already be changed, and thus doesn't need an update */
            .catch(error => console.error(error)); // TODO: handle error
        resetGlobalChecked();
    }

    /**
     * Resets the global check if all checks are set to the
     * given check; does not update if all checks are set to
     * the opposite check.
     *
     * @param check The old check.
     */
    function resetGlobalChecked(check: boolean = globalCheck): void
    {
        if (topicItems.every(item_ => item_.checked === check))
            setGlobalCheck(!check);
    }

    /**
     * Removes the provided topic from the server and awaits
     * an ok result.
     *
     * @param topic The topic to be removed.
     */
    async function remove(topic: Topic): Promise<void>
    {
        await axios.delete('/Topic/' + topic.id)
            .then(() => topicItems.filter(item => item.topic.id !== topic.id))
            .catch(error => console.error(error)); // TODO: handle error
    }

    function flipTopicChecked(item: TopicItem): void
    {
        item.checked = !item.checked;
        setTopicItems(topicItems.slice());
        resetGlobalChecked();
    }

    function setAllTopicChecks(check: boolean): void
    {
        topicItems.forEach(item => (item.checked = check));
        setGlobalCheck(!check);
    }

    function removeSelected(): void
    {
        const removeableItems = topicItems.filter(item => item.checked);
        for (const item of removeableItems)
            remove(item.topic);
        fetch();
    }

    function renderRow(item: TopicItem): JSX.Element
    {
        if (editableItem !== item) {
            return (
                <Checkbox.Item
                    mode='ios'
                    label={item.topic.name}
                    status={item.checked ? 'checked' : 'unchecked'}
                    // onPress={() => flipTopicCheck(i)}
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
                        defaultValue={item.topic.name}
                        label='Name'
                        onChangeText={name => (item.topic.name = name)}
                        />
                    <Button
                        style={style.rowItem}
                        mode="contained"
                        onPress={() => update(item.topic)}
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
                        onPress={() => setAllTopicChecks(globalCheck)}
                        >
                        { globalCheck ? 'Check all' : 'Uncheck all' }
                    </Button>
                    <Button
                        style={style.rowItem}
                        onPress={() => removeSelected()}
                        >
                        Remove selection
                    </Button>
                </View>
                <View>
                {
                    topicItems.map((item, i) =>
                        <Pressable
                            key={i}
                            onPress={() => flipTopicChecked(item)}
                            onLongPress={() => setEditableItem(item)}
                            >
                            { renderRow(item) }
                        </Pressable>
                    )
                }
                </View>
                <View style={style.row}>
                    <TextInput
                        style={style.rowLongItem}
                        editable
                        // TODO: maxLength={?}
                        label='Name'
                        onChangeText={name_ => setNewTopicName(name_)}
                        />
                    <Button
                        style={style.rowItem}
                        onPress={() => submit(newTopicName)}
                        >
                        Add topic
                    </Button>
                </View>
            </ScrollView>
        </Loader>
    );
});
