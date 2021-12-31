import React, { Screen, Course, Question, Answer, useState, useUser, axios, displayName, professor } from '@/.';
import { Loader, View, Text, Icon, ItemList } from '@/components';

export default Screen('Profile', ({ nav, params: { email } }) => {
    let [courses, setCourses] = useState<Course[]>([]);
    let [questions, setQuestions] = useState<Question[]>([]);
    let [answers, setAnswers] = useState<Answer[]>([]);
    let [study, setStudy] = useState('');

    let user: string = useUser().username;
    if (!email) email = user;
    let owner = email === user;
    let prof = professor(email);

    const info = async () => {
        return axios.get('/FieldOfStudy/user', { params: { email } }).then(res => {
            setStudy(res.data.fullName);
            nav.setParams({ screenTitle: owner ? 'Your Profile' : displayName(email) })
        })
    }

    const course = async () => { // TODO by user
        if (prof) return axios.get('/Course', { params: { email } }).then(res => {
            setCourses(res.data);
        })
    }

    const question = async () => { // TODO by user
        if (owner) return axios.get('/Question', { params: { email } }).then(res => {
            setQuestions(res.data);
        })
    }

    const answer = async () => { // TODO by user
        if (owner) return axios.get('/Answer', { params: { email } }).then(res => {
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
            <Text pad margin children={study}/>
            <Text type='link' pad margin children={email}/>
            <ItemList name={owner ? 'Your' : ''} courses={courses} questions={questions} answers={answers}/>
        </Loader>
    )
})
