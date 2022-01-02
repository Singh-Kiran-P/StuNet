export type Notification = {
    id: number;
    notifierId: number;
    notifier: Question | Answer;
    time: string,
}

export type BaseCourse = {
    id: number;
    name: string;
    number: string;
    profEmail: string;
    courseEmail: string;
    description: string;
}

export type Course = BaseCourse & {
    topics: Topic[];
    channels: Channel[];
    questions: Question[];
}

export type BaseChannel = {
    id: number;
    name: string;
}

export type Channel = BaseChannel & {
    course: BaseCourse;
}

export type BaseMessage = {
	userMail: string;
	body: string;
	time: string;
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
    time: string;
    body: string;
    title: string;
    isAccepted: boolean;
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
	fullName: string;
	isBachelor: boolean;
}
