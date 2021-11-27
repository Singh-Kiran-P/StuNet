import React, { Screen, useToken } from '@/.';

import {
    View,
    Button
} from '@/components';

export default Screen('Profile', ({ params, nav }) => {
    let [_, setToken] = useToken();

    return (
        <View>
            <Button onPress={() => nav.push('EditProfile')} children='EditProfile'/>
            <Button onPress={() => setToken('')} children='Logout'/>
        </View>
    )
})
