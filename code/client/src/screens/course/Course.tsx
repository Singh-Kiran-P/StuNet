import React, { Screen } from '@/.';

import {
    View,
    Button
} from '@/components';

export default Screen('Course', ({ params, nav }) => {



    return (
        <View>
            <Button onPress={() => nav.push('Question', { id: 0 })} children='Question'/>
            <Button onPress={() => nav.push('AskQuestion', { courseId: 0 })} children='AskQuestion'/>
        </View>
    )
})
