import React, { Screen } from '@/.';
import { View, Button } from '@/components';

export default Screen('Home', ({ nav }) => {

    return (
        <View>
            <Button children='Course' onPress={() => nav.push('Course', { id: 1 })}/>
            <Button margin children='Question' onPress={() => nav.push('Question', { id: 1 })}/>
        </View>
    )
})
