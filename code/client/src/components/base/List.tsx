// @Kiran
import React, { Screen, useState, useNav } from '@/.';
import { View } from '@/components';
import { Course } from '@/screens/course/Course';
import { List } from 'react-native-paper';

interface IProps {
    courses: Course[],
}

const List_ = ({ courses }: IProps) => {
    let nav = useNav();
    return (
        <View>
            {courses.map((course, i) => {
                return (
                    <List.Item key={i}
                        title={course.name}
                        description={course.number}
                        onPress={() => nav.push('Course', { id: course.id })}
                        left={props => <List.Icon {...props} icon="book" />}
                    // right={props => <Button
                    //     key={i}
                    //     onPress={() => nav.push('Course', { id: course.id })}

                    //     children={"->"} />}

                    >
                    </List.Item>
                );
            })}
        </View>
    );
}

export default List_
