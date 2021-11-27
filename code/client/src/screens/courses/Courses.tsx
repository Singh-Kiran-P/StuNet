import React, { Screen, useState } from '@/.';

import { View, Button, SearchBar, Text } from '@/components';
import List_ from '@/components/base/List';
import { Course } from '../course/Course';
export interface IState {
    courses: Course[];
}

export default Screen('Courses', ({ params, nav }) => {
    const [query, setQuery] = useState('');
    const [courses, setCourses] = useState<IState["courses"]>([]);
    const search = () => {
        setCourses(
            courses.concat([
                {
                    id: 12,
                    name: query,
                    number: 'dsf',
                    questions: [],
                    topics: [],
                }, {
                    id: 12,
                    name: query,
                    number: 'dsf',
                    questions: [],
                    topics: [],
                }, {
                    id: 12,
                    name: query,
                    number: 'dsf',
                    questions: [],
                    topics: [],
                }, {
                    id: 12,
                    name: query,
                    number: 'dsf',
                    questions: [],
                    topics: [],
                }, {
                    id: 12,
                    name: query,
                    number: 'dsf',
                    questions: [],
                    topics: [],
                }, {
                    id: 12,
                    name: query,
                    number: 'dsf',
                    questions: [],
                    topics: [],
                }, {
                    id: 12,
                    name: query,
                    number: 'dsf',
                    questions: [],
                    topics: [],
                }, {
                    id: 12,
                    name: query,
                    number: 'dsf',
                    questions: [],
                    topics: [],
                }, {
                    id: 12,
                    name: query,
                    number: 'dsf',
                    questions: [],
                    topics: [],
                }, {
                    id: 12,
                    name: query,
                    number: 'dsf',
                    questions: [],
                    topics: [],
                }, {
                    id: 12,
                    name: query,
                    number: 'dsf',
                    questions: [],
                    topics: [],
                },
            ]),
        );
    };

    return (
        <View>
            <SearchBar placeholder="sdf" onChangeText={q => setQuery(q)} />
            <Button onPress={search}>Search</Button>

            <List_ courses={courses} nav={nav} />
        </View>
    );
});
