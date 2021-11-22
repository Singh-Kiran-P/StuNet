import React, { Screen } from '@/.';

import {
    View,
    Button
} from '@/components';

export default Screen('Notifications', ({ params, nav }) => {



    return (
        <View>
            <Button onPress={() => nav.push('Question', { id: 0 })} children='Question'/>
        </View>
    )
})
