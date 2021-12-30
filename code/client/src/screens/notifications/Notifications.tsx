import React, { Screen, useState, Notification, BaseTopic, axios, show } from '@/.';
import { Text, List, Loader, CompactNotification } from '@/components';

type Notifications = (Notification & {
    id: number,
    timestamp: number,
    isQuestion: boolean,
    isAccepted: boolean,
    topics: BaseTopic[]
})[]

export default Screen('Notifications', () => {
    let [notifications, setNotifications] = useState<Notifications>([]);
    let [error, setError] = useState('');

    const fetch = async () => {
        return axios.get('/Notification').then(res => {
            let notifications: Notification[][] = [res.data.item1, res.data.item2];
            setNotifications(notifications.map((l, i) => l.map(n => ({
                ...n,
                topics: [],
                isQuestion: !i,
                id: n.notifierId,
                isAccepted: false,
                timestamp: new Date(n.time).getTime()
            }))).flat().sort((a, b) => a.timestamp - b.timestamp));
        }).catch(show(setError));
    }

    return (
        <Loader load={fetch}>
            <Text type='error' pad='top' margin='bottom' hidden={!error} children={error}/>
            <Text type='hint' size='normal' pad='top' margin='bottom' hidden={notifications.length} children="You don't have any notifications yet."/>
            <List data={notifications} renderItem={({ item }) => {
                if (item.isQuestion) return <CompactNotification question={item}/>
                else return <CompactNotification answer={item}/>
            }} />
        </Loader>
    )
})
