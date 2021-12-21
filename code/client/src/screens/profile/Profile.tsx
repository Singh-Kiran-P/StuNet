import React, { Screen, useToken } from '@/.';
import { View, Text, Button } from '@/components';

export default Screen('Profile', () => {
    let [_, setToken] = useToken();

    return (
        <View flex>
            <Text children='TODO show info'/>
            <Text children='TODO show your courses and questions'/>
            <Button align='bottom' icon='logout' children='Logout' onPress={() => setToken('')}/>
        </View>
    )
})
