import React, { Screen, Style, Theme, dateString } from '@/.';

import {
    Text,
    View
} from '@/components';

export default Screen('Answer', ({ params }) => {
    const s = Style.create({

        header: {
            marginBottom: Theme.margin,
            flexDirection: 'row',
            alignItems: 'center',
            flexWrap: 'wrap'
        },

        right: {
            marginLeft: 'auto'
        }

    })

    return (
        <View>
            <View style={s.header}>
                <Text type='header' children={params.title}/>
                <Text type='hint' style={s.right} children={dateString(params.dateTime)}/>
            </View>
            <Text children={params.body}/>
        </View>
    )
})
