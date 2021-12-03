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

export default Screen('EditTopics', ({ params, nav }) => {
    const [topicItems, setTopics] = useState<Array<TopicItem>>([]);
    const [globalCheck, setCheckAll] = useState<boolean>(true);
    const [editableItem, setEditableItem] = useState<TopicItem|null>(null)

    function init(data: Course): void
    {
        setTopics(data.topics.map(topic => ({ topic: topic, checked: false } as TopicItem)));
    }

    async function fetch() //: Promise<void | AxiosResponse<any, any>>
    {
        return axios.get('/Course/' + params.courseId).then(res => init(res.data));
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
            </ScrollView>
        </Loader>
    );
});
