import React, { Screen, Course, Question, useState, axios } from '@/.';
import { Loader, Text, Button, ItemList } from '@/components';

export default Screen('Home', ({ nav }) => {
    let [courses, setCourses] = useState<Course[]>([]);
    let [questions, setQuestions] = useState<Question[]>([]);

    const course = async () => {
        return axios.get('/Course/subscribed').then(res => {
            setCourses(res.data);
        })
    }

    const question = async () => {
        return axios.get('/Question/subscribed').then(res => {
            setQuestions(res.data);
        })
    }

    const fetch = async () => Promise.all([course(), question()]);

    let empty = !courses.length && !questions.length;

    return (
        <Loader load={fetch}>
            <Text type='header' color='accent' pad='top' hidden={!empty} children='Welcome to StuNet!'/>
            <Text pad margin hidden={!empty} children='When you subscribe to courses and questions they will show up here'/>
            <Button pad='top' hidden={!empty} icon='book-search' children='Search for courses' onPress={() => nav.navigate('TabCourses')}/>
            <ItemList name='Subscribed' courses={courses} questions={questions}/>
        </Loader>
    )
})
