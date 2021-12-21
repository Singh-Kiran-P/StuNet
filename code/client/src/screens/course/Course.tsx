import React, { Screen, EmptyCourse, useState, axios } from '@/.';
import { Text, Button, Loader, ScrollView, CompactChannel } from '@/components';

export default Screen('Course', ({ params: { id }, nav }) => {
    let [course, setCourse] = useState(EmptyCourse);

    const fetch = async () => {
        return axios.get('/Course/' + id).then(res => {
            setCourse(res.data);
            nav.setParams({ name: res.data.name });
        })
    }

    return (
        <Loader load={fetch}>
            <Text children={course.description || 'TODO description'}/>
            <Button margin='top-2' icon='comment-multiple' children='Questions' onPress={() => nav.push('Questions', { course })}/>
            <ScrollView content padding='vertical' flex>
                {course.channels?.map((channel, i) => <CompactChannel margin='bottom' key={i} channel={channel}/>)}
            </ScrollView>
            <Button align='bottom' icon='pencil' children='Edit course' onPress={() => nav.push('EditCourse', { course })}/>
        </Loader>
    )
})
