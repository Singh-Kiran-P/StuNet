import React, { Screen, EmptyCourse, Question, useState, axios } from '@/.';
import { Text, Button, Loader } from '@/components';

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
            
            <Button align='bottom' icon='pencil' children='Edit course' onPress={() => nav.push('EditCourse', { course })}/>
        </Loader>
    )
})

/* TODO CHANNELS

<Collapse title='Channels'>
    {channels.map((channel, i) => (
        <Button key={i}
            onPress={() => nav.push('TextChannel', { course: name, channel: channel, scroll: false } )}
            children={channel.name}
        />
    ))}
</Collapse>

*/
