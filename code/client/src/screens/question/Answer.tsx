import React, { Screen, dateString } from '@/.';
import { Text, View } from '@/components';

export default Screen('Answer', ({ params }) => {
    return (
        <View>
            <View type='header'>
                <Text type='header' children={params.title}/>
                <Text type='hint' alignRight children={dateString(params.dateTime)}/>
            </View>
            <Text margin children={params.body}/>
        </View>
    )
})
