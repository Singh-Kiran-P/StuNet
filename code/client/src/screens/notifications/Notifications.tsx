import React, { Screen, useState, Notification, axios, BaseQuestion, BaseAnswer, extend } from '@/.';
import { View, Text, Loader, List, CompactQuestion, CompactAnswer } from '@/components';

type Props = {
    notif: BaseQuestion | BaseAnswer,
    isQuestion: boolean
}


const CompactNotificatio = extend<typeof View, Props>(View, ({ notif, isQuestion, ...props }) => {
    const s = isQuestion ? 'A question has been asked:' : 'An answer has been given:'

    return (
        <View {...props}>
            <Text>{s}</Text>
            {isQuestion ? <CompactQuestion question={notif as BaseQuestion} /> : <CompactAnswer answer={notif as BaseAnswer} />}
        </View>
            
    )
});


export default Screen('Notifications', ({ nav }) => {
    const [Notifications, setNotifications] = useState<[BaseQuestion | BaseAnswer, boolean][]>([]);
    
    const fetch = () => {
        return axios.get('/Notification').then(res => {
            const [questions, answers] = [res.data.item1, res.data.item2];
            const questionTuples = questions.map((n: Notification) => (
                [[{ id: n.notifierId, title: n.title, body: n.body, time: n.time, topics: [] }, true], (new Date(n.time)).getTime()]
            ));
            const answerTuples = answers.map((n: Notification) => (
                [[{ id: n.notifierId, title: n.title, body: n.body, time: n.time }, false], (new Date(n.time)).getTime()]
            ));

            let l = [...questionTuples, ...answerTuples];
            l.sort((a, b) => b[1] - a[1]);

            setNotifications(l.map(n => n[0]));

        }).catch(err => console.log(err))
    }

    return (
        <Loader load={fetch}>
            {Notifications.map(([notif, isQuestion], i) => <CompactNotificatio notif={notif} isQuestion={isQuestion} key={i} />)}
        </Loader>
    )
})
