import React, { Screen, useToken } from '@/.';
import { View, Button } from '@/components';

export default Screen('Profile', ({ nav }) => {
    let [_, setToken] = useToken();

    return (
        <View>
            <Button children='Edit Profile' onPress={() => nav.push('EditProfile')}/>
            <Button margin children='Log out' onPress={() => setToken('')}/>
        </View>
    )
})
