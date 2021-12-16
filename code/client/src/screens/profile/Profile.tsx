import React, { Screen, useToken } from '@/.';
import { View, Text, Button } from '@/components';

export default Screen('Profile', ({ nav }) => {
    let [_, setToken] = useToken();

    return (
        <View flex>
            <Text children='TODO show info'/>
            <Text children='TODO show your courses and questions'/>
            <Button align='bottom' children='Log out' onPress={() => setToken('')}/>
        </View>
    )
})
