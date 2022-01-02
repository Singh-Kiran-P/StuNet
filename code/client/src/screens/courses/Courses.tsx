import React, { Screen, Course, useState, useEffect, useEmail, axios, show, professor } from '@/.';
import { View, Text, Fab, List, CompactCourse } from '@/components';

export default Screen('Courses', ({ nav, params: { search, update } }) => {
    let [courses, setCourses] = useState<Course[]>([]);
    let [refresh, setRefresh] = useState(true);
    let [error, setError] = useState('');
    let prof = professor(useEmail());

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
            <Fab pad='bottom' icon='book-plus' hidden={!prof} onPress={() => nav.push('CreateCourse')}/>
        </View>
    )
})
