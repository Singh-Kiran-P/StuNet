import React, { Screen, Course, useState, useEffect, axios, show } from '@/.';
import { View, Text, List, Button, CompactCourse } from '@/components';

export default Screen('Courses', ({ nav, params: { search, update } }) => {
    let [courses, setCourses] = useState<Course[]>([]);
    let [refresh, setRefresh] = useState(true);
    let [error, setError] = useState('');

    useEffect(() => {
        if (!refresh) setRefresh(true);
        axios.get('/Course/search', { params: { name: search } }).then(
            res => (setRefresh(false), setCourses(res.data)),
            show(setError)
        )
    }, [search, update]);

    return (
        <View flex>
            <Text type='error' pad='top' margin='bottom' hidden={!error} children={error}/>
            <Text type='hint' size='normal' pad='top' margin='bottom' hidden={courses.length} children='No courses match your search'/>
            <List inner padding data={courses} refreshing={refresh} renderItem={course =>
                <CompactCourse margin={!!course.index} course={course.item}/>}
            />
            <Button align='bottom' pad='bottom' icon='book-plus' children='Create Course' onPress={() => nav.push('CreateCourse')}/>
        </View>
    )
})
