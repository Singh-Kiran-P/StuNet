import React, { Screen, EmptyAnswer, useState, useTheme, Style, axios, show, dateString } from '@/.';
import { View, Text, Icon, Loader, CompactQuestion } from '@/components';
import { FAB } from 'react-native-paper'; // TODO in header?

export default Screen('Answer', ({ nav, params: { id } }) => {
    let [answer, setAnswer] = useState(EmptyAnswer);
    let [error, setError] = useState('');
    let [theme] = useTheme();

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

    const s = Style.create({
        fab: {
            backgroundColor: answer.isAccepted ? theme.error : theme.accent,
            position: 'absolute',
            bottom: 0,
            right: 0
        }
    })

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
            <FAB style={s.fab} icon={answer.isAccepted ? 'close' : 'check'} color={theme.bright} onPress={accept}/>
        </Loader>
    )
})
