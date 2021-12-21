import React, { Screen, EmptyCourse, CourseSubscription, useState, useEffect, axios } from '@/.';
import { Text, Button, Loader, ScrollView, CompactChannel } from '@/components';

export default Screen('Course', ({ params: { id, subscribe }, nav }) => {
    let [course, setCourse] = useState(EmptyCourse);
    let [notificationsEnabled, setNotifactionsEnabled] = useState<boolean>(true);

    const fetch = async () => {
        return axios.get('/Course/' + id).then(res => {
            setCourse(res.data);
            nav.setParams({ name: res.data.name });
        }).then(() => { 
            axios.get('/CourseSubscription/ByUserAndCourseId/' + id).then(res => setNotifactionsEnabled(res.data.length > 0))
        });
    }

    // TODO: Move this functionality to the server side
    function toggleNotificationSubcription(data: CourseSubscription[]): void {
        if (data.length === 0) {
            axios.post('/CourseSubscription/', { courseId: id } as CourseSubscription)
            .then(() => setNotifactionsEnabled(!notificationsEnabled))
            .catch(error => console.error(error));
        }
        else {
            axios.delete('/CourseSubscription/' + data[0].id)
            .then(() => setNotifactionsEnabled(!notificationsEnabled))
            .catch(error => console.error(error));
        }
        
    }

    useEffect(() => {
        if (subscribe === null) return;
        axios.get('/CourseSubscription/ByUserAndCourseId/' + id) // TODO test
            .then(response => toggleNotificationSubcription(response.data))
            .catch(error => console.error(error));
    }, [subscribe]);

    return (
        <Loader load={fetch}>
            <Text children={course.description}/>
            <Button margin='top-2' icon='comment-multiple' children='Questions' onPress={() => nav.push('Questions', { course })}/>
            <ScrollView content padding='vertical' flex>
                {course.channels?.map((channel, i) => <CompactChannel margin='bottom' key={i} channel={channel}/>)}
            </ScrollView>
            <Button align='bottom' icon='pencil' children='Edit course' onPress={() => nav.push('EditCourse', { course })}/>
        </Loader>
    )
})
