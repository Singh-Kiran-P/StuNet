import React, { Screen, EmptyAnswer, useState, useTheme, axios, dateString } from '@/.';
import { View, Text, Button, Icon, Loader, CompactQuestion } from '@/components';

import { FAB } from "react-native-paper";

export default Screen('Answer', ({ nav, params: { id } }) => {
    let [answer, setAnswer] = useState(EmptyAnswer);
    let theme = useTheme()[0];

    const fetch = async () => {
        return axios.get('/Answer/' + id).then(res => {
            setAnswer(res.data);
            nav.setParams({ course: res.data.question?.course?.name || '' });
        })
    }

    const accept = (): void => {
        axios.put('/Answer/SetAccepted/' + id + '?accepted=' + !answer.isAccepted)
            .then(() => setAnswer({...answer, isAccepted: !answer.isAccepted}))
            .catch(e => console.error(e));
    }

    return (
        <Loader load={fetch}>
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
            <FAB
                style={{ position: 'absolute', margin: 16, right: 0, bottom: 0, backgroundColor: !answer.isAccepted ? 'green' : 'red' }}
                icon={!answer.isAccepted ? 'check' : 'close'}
                onPress={ () => accept() }
            />
        </Loader>
    )
})
