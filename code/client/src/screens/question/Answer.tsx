import React, { Screen, EmptyAnswer, useState, axios, show, dateString } from '@/.';
import { View, Text, Icon, Fab, Loader, CompactQuestion } from '@/components';

export default Screen('Answer', ({ nav, params: { id } }) => {
    let [answer, setAnswer] = useState(EmptyAnswer);
    let [error, setError] = useState('');

    const fetch = async () => {
        return axios.get('/Answer/' + id).then(res => {
            setAnswer(res.data);
            nav.setParams({ course: res.data.question?.course?.name || '' });
        })
    }

    const accept = () => {
        axios.put('/Answer/SetAccepted/' + id + '?accepted=' + !answer.isAccepted).then(
            () => setAnswer({ ...answer, isAccepted: !answer.isAccepted }),
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
            <View type='row' margin>
                <Icon sizing='large' margin='right-0.5' coloring='accent' name='download'/>
                <Text type='link' {...{}/* TODO attachments */}>
                    Download 3 Attachments
                </Text>
            </View>
            <Fab background={answer.isAccepted && 'error'} icon={answer.isAccepted ? 'close' : 'check'} onPress={accept}/>
        </Loader>
    )
})
