export type Notification = {
    id: number;
    time: string;
    notifierId: number;
    notifier: Question | Answer;
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
    lastMessage?: BaseMessage;
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
    user: {
        email: string;
    }
}

export type Question = BaseQuestion & {
    course: BaseCourse;
}

export type BaseAnswer = {
    id: number;
    time: string;
    body: string;
    title: string;
    isAccepted: boolean;
    user: {
        email: string;
    }
}

export type Answer = BaseAnswer & {
    question: Question;
}

export type Field = {
	id: number;
	name: string;
	fullName: string;
	isBachelor: boolean;
}
