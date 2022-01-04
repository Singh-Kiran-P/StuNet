import React, { Screen, Course, Question, Answer, useState, useEffect, useEmail, axios, displayName, professor } from '@/.';
import { Loader, View, Text, Icon, ItemList } from '@/components';

export default Screen('Profile', ({ nav, params: { email } }) => {
    let [courses, setCourses] = useState<Course[]>([]);
    let [questions, setQuestions] = useState<Question[]>([]);
    let [answers, setAnswers] = useState<Answer[]>([]);
    let [study, setStudy] = useState('');

    let user = useEmail();
    if (!email) email = user;
    let owner = email === user;
    let prof = professor(email);

    useEffect(() => {
        nav.setParams({ screenTitle: owner ? 'Your Profile' : displayName(email) });
    }, []);

    const info = async () => {
        if (!prof) return axios.get('/FieldOfStudy/user', { params: { email } }).then(res => {
            setStudy(res.data.fullName);
        })
    }

    const course = async () => {
        if (prof) return axios.get('/Course/getCreatedCoursesByEmail', { params: { email } }).then(res => {
            setCourses(res.data);
        })
    }

    const question = async () => {
        if (owner) return axios.get('/Question/getAskedQuestionsByEmail', { params: { email } }).then(res => {
            setQuestions(res.data);
        })
    }

    const answer = async () => {
        if (owner) return axios.get('/Answer/getGivenAnswersByEmail', { params: { email } }).then(res => {
            setAnswers(res.data)
        })
    }

    const fetch = async () => Promise.all([info(), course(), question(), answer()]);

    return (
        <Loader load={fetch} flex>
            <View type='header' pad='top'>
                <Icon sizing='massive' coloring='foreground' margin='right' name={prof ? 'account-tie' : 'account'}/>
                <Text type='header' children={prof ? 'Professor' : 'Student'}/>
            </View>
            <Text pad margin hidden={!study} children={study}/>
            <Text type='link' pad margin children={email}/>
            <ItemList name={owner ? 'Your' : ''} courses={courses} questions={questions} answers={answers}/>
        </Loader>
    )
})
