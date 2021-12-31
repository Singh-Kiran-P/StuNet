import React, { Screen, useState, useUser, displayName, professor } from '@/.';
import { Loader, View, Text, Icon } from '@/components';

export default Screen('Profile', ({ nav, params: { email } }) => {
    let [study, setStudy] = useState('');
    let user: string = useUser().username;
    if (!email) email = user;
    let owner = email === user;
    let prof = professor(email);

    const fetch = async () => { // TODO get user by email
        return new Promise<void>(res => res()).then(res => {
            setStudy('Test Study');
            nav.setParams({ screenTitle: owner ? 'Your Profile' : displayName(email) })
        })
    }

    return (
        <Loader load={fetch} flex>
            <View type='header'>
                <Icon sizing='massive' coloring='foreground' margin='right' name={prof ? 'account-tie' : 'account'}/>
                <Text type='header' children={prof ? 'Professor' : 'Student'}/>
            </View>
            <Text margin children={study}/>
            <Text type='link' margin children={email}/>
            <Text align='bottom' children='TODO load field of study'/>
            <Text children='TODO show your courses and questions'/>
        </Loader>
    )
})
