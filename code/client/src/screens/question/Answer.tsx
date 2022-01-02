import React, { Screen, EmptyAnswer, useState, useEmail, axios, show, dateString, professor } from '@/.';
import { View, Text, Fab, Loader, CompactQuestion } from '@/components';
import { update } from '@/nav';

export default Screen('Answer', ({ nav, params: { id } }) => {
    let [answer, setAnswer] = useState(EmptyAnswer);
    let [error, setError] = useState('');
    let email = useEmail();
    let auth = email === answer.question.user.email || professor(email)

    const fetch = async () => {
        return axios.get('/Answer/' + id).then(res => {
            setAnswer(res.data);
            console.log(res.data)
            nav.setParams({ course: res.data.question?.course?.name || '' });
        })
    }

    const accept = () => {
        axios.put('/Answer/SetAccepted/' + id + '?accepted=' + !answer.isAccepted).then(
            () => {
                setAnswer({ ...answer, isAccepted: !answer.isAccepted })
                update('Question')
            },
            show(setError)
        )
    }

    return (
        <Loader load={fetch}>
            <Text type='error' margin='bottom' hidden={!error} children={error}/>
            <CompactQuestion question={answer.question}/>
            <View type='header' margin>
                <Text type='header' children={answer.title}/>
                <Text type='hint' align='right' children={dateString(answer.time)}/>
            </View>
            <Text margin children={answer.body}/>
            <Fab background={answer.isAccepted && 'error'} icon={answer.isAccepted ? 'close' : 'check'} hidden={!auth} onPress={accept}/>
        </Loader>
    )
})
