import React, { Screen, EmptyCourse, useState, useEffect, axios, update, show } from '@/.';
import { Text, Fab, Button, Loader, ScrollView, CompactChannel } from '@/components';

export default Screen('Course', ({ nav, params: { id, subscribe } }) => {
    let [subscribed, setSubscribed] = useState<null | number>(null);
    let [course, setCourse] = useState(EmptyCourse);
    let [error, setError] = useState('');

    const info = async () => {
        axios.get('/Course/' + id).then(res => {
            setCourse(res.data);
            nav.setParams({ name: res.data.name });
        })
    }

    const subscription = async () => {
        axios.get('/CourseSubscription/ByUserAndCourseId/' + id).then(res => {
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
            <Text type='link' pad='top' children={course.courseEmail || 'cnet@uhasselt.be'}/>
            <Text pad='top' children={course.description}/>
            <Button pad='top' icon='comment-multiple' children='Questions' onPress={() => nav.push('Questions', { course })}/>
            <ScrollView inner padding flex children={course.channels?.map((channel, i) =>
                <CompactChannel margin='bottom' key={i} channel={channel}/>)}
            />
            <Fab pad='bottom' icon='pencil' onPress={() => nav.push('EditCourse', { course })}/>
        </Loader>
    )
})
