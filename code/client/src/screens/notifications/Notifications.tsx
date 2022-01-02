import React, { Screen, useState, Notification, BaseTopic, axios, show, timeSort } from '@/.';
import { Text, List, Loader, CompactNotification } from '@/components';

type Notifications = (Notification & {
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
            const unique = (id: number, i: number) => id * notifications.length + i;
            setNotifications(timeSort(notifications.map((l, i) => l.map(n => ({
                isAccepted: false,
                topics: [],
                ...n,
                isQuestion: !i,
                id: unique(n.id, i)
            }))).flat()));
        }).catch(show(setError));
    }

    return (
        <Loader load={fetch}>
            <Text type='error' pad='top' margin='bottom' hidden={!error} children={error}/>
            <Text type='hint' size='normal' pad='top' margin='bottom' hidden={notifications.length} children="You don't have any notifications"/>
            <List inner padding data={notifications} renderItem={({ item, index }) => {
                let id = { ...item, id: item.notifierId };
                if (item.isQuestion) return <CompactNotification question={id} margin={!!index}/>
                else return <CompactNotification answer={id} margin={!!index}/>
            }} />
        </Loader>
    )
})
