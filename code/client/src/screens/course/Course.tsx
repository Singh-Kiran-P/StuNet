import React, { Screen, EmptyCourse, Question, useState, axios } from '@/.';
import { Text, List, Button, Loader, CompactQuestion, SelectTopics } from '@/components';

export default Screen('Course', ({ params, nav }) => {
    let [course, setCourse] = useState(EmptyCourse);
    let [actives, setActives] = useState<number[]>([]);

    const fetch = async () => {
        return axios.get('/Course/' + params.id).then(res => {
            setCourse(res.data);
            nav.setParams({ name: res.data.name });
        })
    }

    const display = (question: Question) => actives.every(i => question.topics.find(t => t.id === i));

    return (
        <Loader load={fetch}>
            <Text children={course.description || 'TODO description'}/>
            <SelectTopics margin topics={course.topics} actives={actives} setActives={setActives}/>
            <Button margin='top-2' icon='comment-plus' children='Ask a question' onPress={() => nav.push('AskQuestion', { course, selected: actives })}/>
            <Text type='hint' size='normal' margin='top-2' hidden={course.questions.filter(display).length} children='No questions match these topics'/>
            <List margin='vertical-2' data={course.questions} renderItem={({ item, index }) => !display(item) ? null : (
                <CompactQuestion margin={!!index} question={item} selected={actives}/>
            )}/>
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
