import React, { axios, Screen, useState, useEffect, Course, errorString } from '@/.';
import { View, Text, List, Button, SearchBar } from '@/components';

export default Screen('Courses', ({ nav }) => {
    const [error, setError] = useState('');
    const [courses, setCourses] = useState<Course[]>([]);

    const search = (query: string) => {
        axios.get('/Course/search', { params: { name: query } }).then(res => {
            setCourses(res.status === 200 ? res.data : []);
        }).catch(err => setError(errorString(err)));
    }

    useEffect(() => search(''), []); // TODO fuzzy substring matching in backend, so the empty string matches all courses

    return (
        <View flex>
            <SearchBar placeholder='Search courses' onSearch={search}/>
            <Text type='error' margin hidden={!error} children={error}/>
            <Text type='hint' size='normal' margin hidden={courses.length} children='No courses match your search'/>
            <List data={courses} renderItem={course => ( // TODO test
                <List.Item
                    title={course.item.name}
                    description={course.item.number}
                    onPress={() => nav.push('Course', { id: course.item.id })}
                    left={props => <List.Icon {...props} icon='book'/>}
                />
            )}/>
            <Button align='bottom' onPress={() => nav.push('CreateCourse')} children='Create Course'/>
        </View>
    )
})
