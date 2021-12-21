export type BaseCourse = {
    id: number;
    name: string;
    number: string;
}

export type Course = BaseCourse & {
    topics: Topic[];
    questions: Question[];
    channels: Channel[];
}

export type BaseChannel = {
    id: number;
    name: string;
}

export type Channel = BaseChannel & {}

export type BaseMessage = {
	userMail: string,
	body: string,
	time: string
}

export type Message = BaseMessage & {}

export type BaseTopic = {
    id: number;
    name: string;
}

export type Topic = BaseTopic & {
    course: BaseCourse;
    questions: BaseQuestion[];
}

export type BaseQuestion = {
    id: number;
    title: string;
    body: string;
    time: string;
    topics: BaseTopic[];
}

export type Question = BaseQuestion & {
    course: BaseCourse;
    user: {
        email: string;
    }
}

export type BaseAnswer = {
    id: number;
    title: string;
    body: string;
    time: string;
}

export type Answer = BaseAnswer & {
    question: BaseQuestion;
    user: {
        email: string;
    }
}

export type Field = {
	id: number;
	name: string;
	year: number;
	fullName: string;
	isBachelor: boolean;
}

export type FOS = {
	field: string;
	degree: string;
	year: string;
}

export const enum User {
	PROF,
	STUDENT
}

export type CourseSubscription = {
    id: number;
    userId: string;
    courseId: number;
    dateTime: string;
}

export type QuestionSubscription = {
    id: number;
    userId: string;
    questionId: number;
    dateTime: string;
}
