import React, { Screen } from '@/.';

import {
    View,
    Button
} from '@/components';

export default Screen('Profile', ({ params, nav }) => {



    return (
        <View>
            <Button onPress={() => nav.push('EditProfile')} children='EditProfile'/>
        </View>
    )
})
