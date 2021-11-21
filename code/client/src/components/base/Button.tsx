import React, { Props } from '@/.';

import {
    Button
} from 'react-native-paper';

export default (props: Partial<Props<typeof Button>>) => (
    <Button mode='contained' {...props as Props<typeof Button>}/>
)
