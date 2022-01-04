import React, { Screen, EmptyCourse, useState, useEffect, useEmail, axios, update, show } from '@/.';
import { Text, Fab, Button, Loader, ScrollView, CompactChannel } from '@/components';

export default Screen('Course', ({ nav, params: { id, subscribe } }) => {
    let [subscribed, setSubscribed] = useState<null | number>(null);
    let [course, setCourse] = useState(EmptyCourse);
    let [error, setError] = useState('');

    let owner = useEmail() === (course.profEmail || NaN);

    const info = async () => {
        return axios.get('/Course/' + id).then(res => {
            setCourse({ ...res.data, channels: res.data.channels?.reverse() });
            nav.setParams({ name: res.data.name });
        })
    }

    const subscription = async () => {
        return axios.get('/CourseSubscription/ByUserAndCourseId/' + id).then(res => {
            setSubscribed(res.data.length ? res.data[0].id : NaN);
            nav.setParams({ subscribe: res.data.length > 0 });
        })
    }

    const fetch = async () => Promise.all([info(), subscription()]);

    useEffect(() => {
        if (subscribe === null) return;
        if (subscribed === null) return;
        if (subscribe === !isNaN(subscribed)) return;
        if (subscribe) axios.post('/CourseSubscription/', { courseId: id }).then(
            res => (setSubscribed(res.data.id), update('Home')),
            show(setError)
        )
        else axios.delete('/CourseSubscription/' + subscribed).then(
            () => (setSubscribed(NaN), update('Home')),
            show(setError)
        )
    }, [subscribe, subscribed]);

    return (
        <Loader load={fetch}>
            <Text type='error' pad='top' hidden={!error} children={error}/>
            <Text type='link' pad='top' children={course.courseEmail}/>
            <ScrollView inner pad='top' flex>
                <Text children={course.description}/>
            </ScrollView>
            <Button pad='top' icon='comment-multiple' children='Questions' onPress={() => nav.push('Questions', { course })}/>
            <Text type='header' color='placeholder' pad='top' hidden={!course.channels?.length} children='Channels'/>
            <ScrollView inner padding='bottom,horizontal' flex children={course.channels?.map((channel, i) =>
                <CompactChannel margin={!!i} key={i} channel={channel}/>)}
            />
            <Fab pad='bottom' icon='pencil' hidden={!owner} onPress={() => nav.push('EditCourse', { course })}/>
        </Loader>
    )
})
