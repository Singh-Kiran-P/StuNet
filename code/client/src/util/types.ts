export type Course = {
    id: number;
    name: string;
    number: string;
    topics: Topic[];
    questions: Question[];
    channels: Channel[];
}

export type Channel = {
    id: number;
    name: string;
}

export type Topic = {
    id: number;
    name: string;
}

export type Question = {
    id: number;
    title: string;
    body: string;
    time: string;
    // TODO topicIds: number[];
}

export type Answer = {
    dateTime: string;
    title: string;
    body: string;
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
