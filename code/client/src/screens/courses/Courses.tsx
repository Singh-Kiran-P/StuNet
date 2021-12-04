import React, { axios, Screen, useState, useEffect, Course, errorString } from '@/.';
import { View, Text, Button, SearchBar, ScrollView } from '@/components';
import { List } from 'react-native-paper';

export default Screen('Courses', ({ nav }) => {
    const [query, setQuery] = useState('');
    const [error, setError] = useState('');
    const [courses, setCourses] = useState<Course[]>([]);

    const search = () => {
        axios.get('/Course/search', { params: { name: query } }).then(res => {
            setCourses(res.status === 200 ? res.data : []);
        }).catch(err => setError(errorString(err)));
    }

    useEffect(() => search(), []);

    return (
        <View>
            <SearchBar placeholder='Search courses' onChangeText={setQuery}/>
            <Button margin onPress={search} children='Search'/>
            <Text type='error' margin hidden={!error} children={error}/>
            <Text type='hint' size='normal' margin hidden={courses.length} children='No courses match your search'/>
            <ScrollView>
                {courses.map((course, i) => (
                    <List.Item key={i}
                        title={course.name}
                        description={course.number}
                        onPress={() => nav.push('Course', { id: course.id })}
                        left={props => <List.Icon {...props} icon='book'/>}
                    />
                ))}
            </ScrollView>
        </View>
    )
})
