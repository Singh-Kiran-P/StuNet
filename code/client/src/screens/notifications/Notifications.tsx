import React, { Screen, useState, Notification, axios } from '@/.';
import { View, Text, Loader, List, CompactQuestion, CompactAnswer } from '@/components';
import { useEffect } from 'react';

export default Screen('Notifications', ({ nav }) => {
    const [[QuestionNotifications, AnswerNotifications], setNotifications] = useState<[Notification[], Notification[]]>([[],[]]);

    const fetch = () => {
        return axios.get('/Notification').then(res => setNotifications([res.data.item1, res.data.item2]))
    }

    return (
        <Loader load={fetch}>
            {QuestionNotifications.map((notif, i) => {
                return <View key={i}>
                    <Text>A question has been asked:</Text>
                    <CompactQuestion question={{id: notif.notifierId, title: notif.title, body: notif.body, time: notif.time, topics: []}}></CompactQuestion>
                </View>
            })}
            {AnswerNotifications.map((notif, i) => {
                return <View key={i}>
                    <Text>An answer has been given:</Text>
                    <CompactAnswer answer={{id: notif.notifierId, title: notif.title, body: notif.body, time: notif.time}}></CompactAnswer>
                </View>
            })}
        </Loader>
    )
})
