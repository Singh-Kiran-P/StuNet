import React, { Screen, Course, Channel, Question, Answer, useState, axios, update } from '@/.';
import { Text, Loader, Button, SectionList, CompactCourse, CompactChannel, CompactQuestion, CompactAnswer } from '@/components';

export default Screen('Home', () => {
    let [courses, setCourses] = useState<Course[]>([]);
    let [channels, setChannels] = useState<Channel[]>([]);
    let [questions, setQuestions] = useState<Question[]>([]);
    let [answers, setAnswers] = useState<Answer[]>([]);
    let [refresh, setRefresh] = useState(true);

    const course = async () => {
        return axios.get('/Course').then(res => {
            setCourses(res.data);
        })
    }

    const channel = async () => {
        return axios.get('/Channel').then(res => {
            setChannels(res.data);
        })
    }

    const question = async () => {
        return axios.get('/Question').then(res => {
            setQuestions(res.data);
        })
    }

    const answer = async () => {
        return axios.get('/Answer').then(res => {
            setAnswers(res.data);
        })
    }

    const fetch = async () => {
        if (!refresh) setRefresh(true);
        return Promise.all([course(), channel(), question(), answer()]).then(
            () => setRefresh(false)
        )
    }

    return (
        <Loader load={fetch}>
            <Text pad='top' children='TODO only show subscribed items'/>
            <Button pad margin children='Update' onPress={() => update('Home')}/>
            <SectionList inner padding='horizontal,bottom' refreshing={refresh} sections={[
                { title: 'Courses', data: courses as any[] },
                { title: 'Channels', data: channels as any[] },
                { title: 'Questions', data: questions as any[] },
                { title: 'Answers', data: answers as any[] }
            ]} renderSectionHeader={({ section }) => (
                <Text type='header' margin='top' children={section.title}/>
            )} renderItem={({ item, section }) => {
                switch (section.title) {
                    case 'Courses': return <CompactCourse course={item}/>
                    case 'Channels': return <CompactChannel channel={item}/>
                    case 'Questions': return <CompactQuestion question={item}/>
                    case 'Answers': return <CompactAnswer answer={item}/>
                    default: return null;
                }
            }}/>
        </Loader>
    )
})

/* TODO temp notification test

import React, { Screen } from '@/.';
import { View, Button } from '@/components';
import notifee, { AndroidImportance } from '@notifee/react-native';

async function onDisplayNotification() {
    // Create a channel
    const channelId = await notifee.createChannel({
        id: 'default',
        name: 'Default Channel',
    });

    try {
        // https://notifee.app/react-native/docs/android/appearance
        // Display a notification
        await notifee.displayNotification({
            title: 'Notification Title',
            body: 'Main body content of the notification',
            android: {
                importance: AndroidImportance.HIGH,
                channelId,
                smallIcon: 'ic_launcher', // optional, defaults to 'ic_launcher'.
            },
        });
    } catch (e) {
        console.log(e)
    }
}

<View>
    <Button children='Course' onPress={() => nav.push('Course', { id: 1 })}/>
    <Button margin children='Question' onPress={() => nav.push('Question', { id: 1 })}/>
    <Button margin children='Notification' onPress={() => onDisplayNotification()}/>
</View>

*/
