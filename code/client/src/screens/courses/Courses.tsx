import React, { Screen, Course, useState, useEffect, axios, show } from '@/.';
import { View, Text, List, Button, CompactCourse } from '@/components';

export default Screen('Courses', ({ nav, params: { search } }) => {
    let [error, setError] = useState('');
    let [courses, setCourses] = useState<Course[]>([]);

    useEffect(() => {
        axios.get('/Course/search', {
            params: { name: search }
        }).then(res => setCourses(res.data), show(setError))
    }, [search]);

    return (
        <View flex>
            <Text type='error' margin='bottom' hidden={!error} children={error}/>
            <Text type='hint' size='normal' margin='bottom' hidden={courses.length} children='No courses match your search'/>
            <List margin='bottom-2' data={courses} renderItem={course => <CompactCourse margin={!!course.index} course={course.item}/>}/>
            <Button align='bottom' icon='book-plus' children='Create Course' onPress={() => nav.push('CreateCourse')}/>
        </View>
    )
})
