import React, { extend, BaseCourse, useNav } from '@/.';
import { View, Text, Icon, Touchable } from '@/components/base';

type Props = {
    course: BaseCourse;
}

export default extend<typeof Touchable, Props>(Touchable, ({ course, ...props }) => {
    let nav = useNav();

    return (
        <Touchable type='row' padding='all-0.2' onPress={() => nav.navigate({ name: 'Course', params: { id: course.id }, merge: true })} {...props}>
            <Icon sizing='huge' coloring='accent' padding='vertical-0.2' name='book'/>
            <View shrink grow margin='left'>
                <View type='header'>
                    <Text type='header' size='normal' children={course.name}/>
                    <Text type='hint' align='right' children={course.number}/>
                </View>
                <Text numberOfLines={3} children={course.description}/>
            </View>
        </Touchable>
    )
})
