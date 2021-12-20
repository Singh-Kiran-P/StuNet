import React, { Screen, axios, useState, Topic, Course, Channel, Question, CourseSubscription } from '@/.';
import { Button, Loader, Collapse, ScrollView, CompactQuestion } from '@/components';

export default Screen('Course', ({ params, nav }) => {
    const [name, setName] = useState('');
    const [number, setNumber] = useState('');
    const [topics, setTopics] = useState<Topic[]>([]);
    const [channels, setChannels] = useState<Channel[]>([]);
    const [questions, setQuestions] = useState<Question[]>([]);
    const [notificationsEnabled, setNotifactionsEnabled] = useState<boolean>(true);

    const init = (data: Course) => {
        setName(data.name);
        setNumber(data.number);
        setTopics(data.topics);
        setQuestions(data.questions);
        setChannels(data.channels)
        nav.setParams({ name: data.name });
    }

    const fetch = async () => {
        axios.get('/Course/' + params.id)
            .then(res => init(res.data))
            .catch(err => {}) // TODO handle error
        axios.get('/CourseSubscription/ByUserAndCourseId/' + params.id)
            .then(response => setNotifactionsEnabled(response.data.length > 0))
            .catch(error => console.error(error));
    }

    //TODO: Move this functionality to the server side
    function toggeNotificationSubcription(data: CourseSubscription[]): void {
        if (data.length === 0) {
            axios.post('/CourseSubscription/', { courseId: params.id } as CourseSubscription)
                .catch(error => console.error(error));
        }
        else {
            axios.delete('/courseSubscription/' + data[0].id)
                .catch(error => console.error(error));
        }
    }

    /**
     * Updates the notification on the server, and if succes
     * updates the local notification.
     */
    function updateNotificationSubscription(): void {
        axios.get('/CourseSubscription/ByUserAndCourseId/' + params.id)
            .then(response => toggeNotificationSubcription(response.data))
            .catch(error => console.error(error));
        setNotifactionsEnabled(!notificationsEnabled);
    }

    console.log(params.id);
    return (
        <Loader load={fetch}>
            <ScrollView>
                <Collapse title='Topics'>
                    {topics.map((topic, i) => ( // TODO: show search results with this topic only on click
                        <Button key={i}
                            onPress={() => nav.push('Course', params)}
                            children={topic.name}
                        />
                    ))}
                </Collapse>
                <Collapse title='Channels'>
                    {channels.map((channel, i) => (
                        <Button key={i}
                            onPress={() => nav.push('textChannel', { course: name, channel: channel, scroll: false } )}
                            children={channel.name}
                        />
                    ))}
                </Collapse>
                <Collapse margin title='Questions'>
                    {questions.map((question, i) => (
                        <CompactQuestion key={i} question={question}/>
                    ))}
                </Collapse>
                <Button margin children='Ask a question' onPress={() => nav.push('AskQuestion', { courseId: params.id })}/>
                <Button margin children='Edit course' onPress={() => nav.push('EditCourse', { id: params.id })}/>
                {/* Temporary button which should be moved to the page header as an icon */}
                <Button margin children={(notificationsEnabled ? 'Disable' : 'Enable') + ' notifications'} onPress={() => updateNotificationSubscription()}/>
            </ScrollView>
        </Loader>
    );
});
