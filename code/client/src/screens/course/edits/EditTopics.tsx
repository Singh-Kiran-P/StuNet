/* eslint-disable curly */
/* eslint-disable jsx-quotes */
import React, {
    Screen,
    axios,
    useState,
    // AxiosResponse, // TODO: reexport from 'axios'
    Course,
    Topic,
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

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export default Screen('EditTopics', ({ params, nav }) => {
    const [topicItems, setTopics] = useState<Array<TopicItem>>([]);
    const [globalCheck, setCheckAll] = useState<boolean>(true);
    const [editableItem, setEditableItem] = useState<TopicItem|null>(null);
    const [newTopicName, setNewTopicName] = useState<string>('');

    function init(data: Course): void
    {
        setTopics(data.topics.map(topic => ({ topic: topic, checked: false } as TopicItem)));
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
        fetch(); /* Thus fetch all to update full list */
    }

    /**
     * Updates the topic in the server.
     *
     * @param topic The topic to be updated.
     */
    function update(topic: Topic): void
    {
        axios.put('/Topic/' + topic.id, { name: topic.name })
            // .then(() => ) /* Topic name will already be changed */
            .catch(error => console.error(error)); // TODO: handle error
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
        setTopics(topicItems.slice());
    }

    function setAllTopicChecks(check: boolean): void
    {
        topicItems.forEach(item => (item.checked = check));
        setCheckAll(!check);
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
                <View>
                    <TextInput
                        editable
                        // TODO: maxLength={?}
                        defaultValue={item.topic.name}
                        // style={s.margin}
                        label='E-mail'
                        onChangeText={name => (item.topic.name = name)}
                        />
                    <Button
                        mode="contained"
                        style={{width: '50%'}}
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
                <Button onPress={() => setAllTopicChecks(globalCheck)}>{ globalCheck ? 'Check all' : 'Uncheck all' }</Button>
                <Button onPress={() => removeSelected()}>Remove selection</Button>
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
                <View>
                    <TextInput
                        editable
                        // TODO: maxLength={?}
                        // style={s.margin}
                        label='Name'
                        onChangeText={name_ => setNewTopicName(name_)}
                        />
                    <Button onPress={() => submit(newTopicName)}>Add topic</Button>
                </View>
            </ScrollView>
        </Loader>
    );
});
