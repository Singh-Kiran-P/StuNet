import React, { Screen, useState, Notification, axios } from '@/.';
import { View, Text, Loader, List, CompactQuestion } from '@/components';
import { useEffect } from 'react';

export default Screen('Notifications', ({ nav }) => {
    const [[QuestionNotifications, AnswerNotifications], setNotifications] = useState<[Notification[], Notification[]]>([[],[]]);

    const fetch = () => {
        return axios.get('/Notification').then(res => setNotifications([res.data.item1, res.data.item2]))
    }

    useEffect(() => {
        console.log(QuestionNotifications)
    }, [QuestionNotifications])

    return (
        <Loader load={fetch}>
            {QuestionNotifications.map((notif, i) => {
                return <View>
                    <Text>{notif.id}</Text>
                    <Text>{notif.notifierId}</Text>
                </View>
            })}
        </Loader>
    )
})
