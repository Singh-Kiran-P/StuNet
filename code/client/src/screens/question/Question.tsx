import React, { Screen, Style, Theme } from '@/.';
import { Dimensions } from 'react-native';

import {
    View,
    Text,
    Link,
    Button,
    ScrollView
} from '@/components';

export default Screen('Question', ({ params, nav }) => {

    const s = Style.create({
        view: {
            flex: 1
        },

        screen: {
            height: 0,
            flexGrow: 1
        },

        body: {
            height: Dimensions.get('window').height / 2
        },

        button: {
            marginTop: Theme.margin
        }
    })

    return (
        <View style={s.view}>
            <Text type='header' children='Titel'/>
            <Text>TODO course, autheur, tijd</Text>
            <ScrollView style={s.screen}>
                <ScrollView style={s.body} nestedScrollEnabled>
                    <Text>Body start{'\n\n\n\n\n\n\n\n\n'}body{'\n\n\n\n\n\n\n\n\n'}body{'\n\n\n\n\n\n\n\n\n'}body{'\n\n\n\n\n\n\n\n\n'}body{'\n\n\n\n\n\n\n\n\n'}body{'\n\n\n\n\n\n\n\n\n'}body{'\n\n\n\n\n\n\n\n\n'}body end</Text>
                    <Link to={{} /* TODO icon */}>Download 3 Attachments</Link>
                </ScrollView>
                    <Button style={s.button} onPress={() => nav.push('CreateAnswer', { questionId: params.id })} children='Answer'/>
                    <Text>Body start{'\n\n\n\n\n\n\n\n\n'}body{'\n\n\n\n\n\n\n\n\n'}body{'\n\n\n\n\n\n\n\n\n'}body{'\n\n\n\n\n\n\n\n\n'}body{'\n\n\n\n\n\n\n\n\n'}body{'\n\n\n\n\n\n\n\n\n'}body{'\n\n\n\n\n\n\n\n\n'}body end</Text>
            </ScrollView>
        </View>
    )
})
