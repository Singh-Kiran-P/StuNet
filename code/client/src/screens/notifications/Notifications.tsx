import React, { Screen, useState, Notification, axios, show, timeSort, Question, Answer } from '@/.';
import { Text, List, Loader, CompactNotification } from '@/components';

type Notifications = (Notification & {
    isQuestion: boolean;
})[];

export default Screen('Notifications', () => {
    let [notifications, setNotifications] = useState<Notifications>([]);
    let [error, setError] = useState('');

    const fetch = async () => {
        return axios.get('/Notification').then(res => {
            let notifications: Notification[][] = [res.data.item1, res.data.item2];
            const unique = (id: number, i: number) => id * notifications.length + i;
            setNotifications(timeSort(notifications.map((l, i) => l.map(n => ({
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
                if (item.isQuestion) return <CompactNotification question={item.notifier as Question} margin={!!index}/>
                else return <CompactNotification answer={item.notifier as Answer} margin={!!index}/>
            }} />
        </Loader>
    )
})
