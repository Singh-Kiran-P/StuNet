import * as T from '@/util/types';

export const EmptyCourse: T.Course = {
    id: 0,
    name: '',
    number: '',
    topics: [],
    questions: []
}

export const EmptyTopic: T.Topic = {
    id: 0,
    name: '',
    course: EmptyCourse,
    questions: []
}


export const EmptyQuestion: T.Question = {
    id: 0,
    title: '',
    body: '',
    time: '',
    topics: [],
    course: EmptyCourse,
    user: {
        email: ''
    }
}

export const EmptyAnswer: T.Answer = {
    id: 0,
    title: '',
    body: '',
    time: '',
    question: EmptyQuestion,
    user: {
        email: ''
    }
}

export const EmptyField: T.Field = {
	id: 0,
	name: '',
	year: 0,
	fullName: '',
	isBachelor: false
}
