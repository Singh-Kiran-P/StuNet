import React, { Screen, EmptyCourse, CourseSubscription, useState, axios } from '@/.';
import { Text, Button, Loader, ScrollView, CompactChannel } from '@/components';

export default Screen('Course', ({ params: { id }, nav }) => {
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

    //TODO: Move this functionality to the server side
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

    /**
     * Updates the notification on the server, and if succes
     * updates the local notification.
     */
    function updateNotificationSubscription(): void {
        axios.get('/CourseSubscription/ByUserAndCourseId/' + id)
            .then(response => toggleNotificationSubcription(response.data))
            .catch(error => console.error(error));
    }


    return (
        <Loader load={fetch}>
            {/* Temporary button which should be moved to the page header as an icon */}
            <Button margin icon={notificationsEnabled ? 'bell' : 'bell-off'} /* children={(notificationsEnabled ? 'Disable' : 'Enable') + ' notifications'} */ onPress={() => updateNotificationSubscription()}/>
            <Text children={course.description || 'TODO description'}/>
            <Button margin='top-2' icon='comment-multiple' children='Questions' onPress={() => nav.push('Questions', { course })}/>
            <ScrollView content padding='vertical' flex>
                {course.channels?.map((channel, i) => <CompactChannel margin='bottom' key={i} channel={channel}/>)}
            </ScrollView>
            <Button align='bottom' icon='pencil' children='Edit course' onPress={() => nav.push('EditCourse', { course })}/>
        </Loader>
    )
})
