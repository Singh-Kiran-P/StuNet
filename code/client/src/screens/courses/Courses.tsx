import React, { Screen, useState } from '@/.';

import { View, Button, SearchBar, Text } from '@/components';
import { Course } from '../course/Course';

interface IState {
    courses: Course[];
}

export default Screen('Courses', ({ params, nav }) => {
    const [query, setQuery] = useState('');
    const [courses, setCourses] = useState<IState["courses"]>([]);
    const search = () => {
        console.log(query);
        setCourses(
            courses.concat([
                {
                    id: 12,
                    name: 'dsf',
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
            {courses.map((course, i) => {
                return (
                    <Button
                        onPress={() => {
                            nav.push('Course', { id: course.id });
                        }}
                        key={i}>
                        {course.name}
                    </Button>
                );
            })}
        </View>
    );
});
