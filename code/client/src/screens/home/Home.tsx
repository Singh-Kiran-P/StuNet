import React, { Screen, Course, Question, useState, axios } from '@/.';
import { Loader, ItemList } from '@/components';

export default Screen('Home', () => {
    let [courses, setCourses] = useState<Course[]>([]);
    let [questions, setQuestions] = useState<Question[]>([]);

    const course = async () => {
        return axios.get('/Course'/* TODO /subscribed */).then(res => {
            setCourses(res.data);
        })
    }

    const question = async () => {
        return axios.get('/Question'/* TODO /subscribed */).then(res => {
            setQuestions(res.data);
        })
    }

    const fetch = async () => Promise.all([course(), question()]);

    return (
        <Loader load={fetch}>
            <ItemList name='Subscribed' courses={courses} questions={questions}/>
        </Loader>
    )
})
