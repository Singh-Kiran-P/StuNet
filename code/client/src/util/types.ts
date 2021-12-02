export type Course = {
    id: number;
    name: string;
    number: string;
    topics: Topic[];
    questions: Question[];
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
    // topicIds: Array<number>;
}

export type Answer = {
    dateTime: string;
    title: string;
    body: string;
}
