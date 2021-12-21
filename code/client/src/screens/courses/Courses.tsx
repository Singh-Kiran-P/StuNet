import React, { Screen, Course, useState, useEffect, axios, show } from '@/.';
import { View, Text, List, Button, CompactCourse } from '@/components';

export default Screen('Courses', ({ nav, params: { search, update } }) => {
    let [error, setError] = useState('');
    let [courses, setCourses] = useState<Course[]>([]);

    useEffect(() => {
        axios.get('/Course/search', {
            params: { name: search }
        }).then(res => setCourses(res.data), show(setError))
    }, [search, update]);

    return (
        <View flex>
            <Text type='error' margin='bottom' hidden={!error} children={error}/>
            <Text type='hint' size='normal' margin='bottom' hidden={courses.length} children='No courses match your search'/>
            <List content padding='vertical' data={courses} renderItem={course => <CompactCourse margin={!!course.index} course={course.item}/>}/>
            <Button align='bottom' margin='bottom-2' icon='book-plus' children='Create Course' onPress={() => nav.push('CreateCourse')}/>
        </View>
    )
})
