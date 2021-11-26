import React, { Screen } from '@/.';

import {
    View,
    Button
} from '@/components';

export default Screen('Home', ({ params, nav }) => {



    return (
        <View>
            <Button onPress={() => nav.push('Course', { id: 1 })} children='Course'/>
            <Button onPress={() => nav.push('Question', { id: 0 })} children='Question'/>
        </View>
    )
})
