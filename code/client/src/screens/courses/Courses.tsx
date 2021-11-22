import React, { Screen } from '@/.';

import {
    View,
    Button
} from '@/components';

export default Screen('Courses', ({ params, nav }) => {

    return (
        <View>
            <Button onPress={() => nav.push('Course', { id: 0 })} children='Course'/>
            <Button onPress={() => nav.push('CreateCourse')} children='CreateCourse'/>
        </View>
    )
})
