import React, { Screen, EmptyAnswer, useState, axios, dateString } from '@/.';
import { View, Text, Icon, Loader, CompactQuestion } from '@/components';

export default Screen('Answer', ({ nav, params: { id } }) => {
    let [answer, setAnswer] = useState(EmptyAnswer);

    const fetch = async () => {
        return axios.get('/Answer/' + id).then(res => {
            setAnswer(res.data);
            nav.setParams({ course: res.data.question?.course?.name || '' });
        })
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
        </Loader>
    )
})
