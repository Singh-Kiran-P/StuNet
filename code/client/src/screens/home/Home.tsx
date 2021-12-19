import React, { Screen, Course, Question, Answer, useState, axios, update } from '@/.';
import { Text, Loader, Button, CompactCourse, CompactQuestion, CompactAnswer } from '@/components';
import { SectionList } from 'react-native';

export default Screen('Home', () => {
    let [courses, setCourses] = useState<Course[]>([]);
    let [questions, setQuestions] = useState<Question[]>([]);
    let [answers, setAnswers] = useState<Answer[]>([]);

    const course = async () => {
        return axios.get('/Course').then(res => {
            setCourses(res.data);
        })
    }

    const question = async () => {
        return axios.get('/Question').then(res => {
            setQuestions(res.data);
        })
    }

    const answer = async () => {
        return axios.get('/Answer').then(res => {
            setAnswers(res.data);
        })
    }

    const fetch = async () => Promise.all([course(), question(), answer()]);

    return (
        <Loader load={fetch}>
            <Text children='TODO only show subscribed items'/>
            <Button margin children='Update' onPress={() => update('Home')}/>
            <SectionList sections={[
                { title: 'Courses', data: courses as any[] },
                { title: 'Questions', data: questions as any[] },
                { title: 'Answers', data: answers as any[] }
            ]} renderSectionHeader={({ section }) => (
                <Text type='header' margin='top-2' children={section.title}/>
            )} renderItem={({ item, section }) => {
                switch (section.title) {
                    case 'Courses': return <CompactCourse course={item}/>
                    case 'Questions': return <CompactQuestion question={item}/>
                    case 'Answers': return <CompactAnswer answer={item}/>
                    default: return null;
                }
            }}/>
        </Loader>
    )
})
