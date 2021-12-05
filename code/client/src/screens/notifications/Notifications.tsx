import React, { Screen } from '@/.';
import { View, Button } from '@/components';

export default Screen('Notifications', ({ nav }) => {

    return (
        <View>
            <Button children='Question' onPress={() => nav.push('Question', { id: 1 })}/>
        </View>
    )
})
