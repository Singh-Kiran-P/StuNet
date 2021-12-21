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
            <Text type='error' margin='bottom' hidden={!error} children={error}/>
            <Text type='hint' size='normal' margin='top-2,bottom' hidden={courses.length} children='No courses match your search'/>
            <List inner padding='vertical' data={[]} refreshing={true} renderItem={course =>
                <CompactCourse margin={!!course.index} course={course.item}/>}
            />
            <Button align='bottom' margin='bottom-2' icon='book-plus' children='Create Course' onPress={() => nav.push('CreateCourse')}/>
        </View>
    )
})
