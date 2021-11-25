import React, { Screen,Style } from '@/.';

import {
    View,
    Button
} from '@/components';

export default Screen('Home', ({ params, nav }) => {

    const s = Style.create({
        button:{
            marginBottom: 15,
        }
    })

    return (
        <View>
            <Button style={s.button} onPress={() => nav.push('Course', { id: 0 })} children='eskjfdkihi'/>
            <Button onPress={() => nav.push('Question', { id: 0 })} children='Question'/>
        </View>
    )
})
