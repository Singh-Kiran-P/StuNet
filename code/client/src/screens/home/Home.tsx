import React, { Screen, update } from '@/.';

import {
    View,
    Button
} from '@/components';

export default Screen('Home', ({ nav }) => {

    return (
        <View>
            <Button onPress={() => nav.push('Course', { id: 1 })} children='Course'/>
            <Button onPress={() => nav.push('Question', { id: 1 })} children='Question'/>
        </View>
    )
})
