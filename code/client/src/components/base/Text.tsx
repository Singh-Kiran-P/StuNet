import React, { Props } from '@/.';

import {
    Text
} from 'react-native-paper';

export default (props: Partial<Props<typeof Text>>) => (
    <Text {...props as Props<typeof Text>}/>
)
