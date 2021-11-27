import React, { Screen, useState } from '@/.';
import { View } from '@/components';
import { Course } from '@/screens/course/Course';
import { FlatList, SafeAreaView, StatusBar, StyleSheet } from 'react-native';
import { Button, List } from 'react-native-paper';

interface IProps {
    courses: Course[],
    nav: any
}
const List_: React.FC<IProps> = ({ courses, nav }) => {

    return (
        <View>
            {courses.map((course, i) => {
                return (
                    <List.Item key={i}
                        title={course.name}
                        description={course.number}
                        // onPress={() => nav.push('CreateCourse', { i })}
                        left={props => <List.Icon {...props} icon="book" />}
                        right={props => <Button
                            key={i}
                            onPress={() => nav.push('CreateCourse', { i })}
                            children={"hello"} />}

                    >
                    </List.Item>
                );
            })}
        </View>
    );
}

export default List_
