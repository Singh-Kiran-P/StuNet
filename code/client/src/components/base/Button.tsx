import { Button } from 'react-native-paper';
import React, { Props } from '@/.';

export default (props: Partial<Props<typeof Button>>) => {
    return <Button
        mode='contained'
        {...props as Props<typeof Button>}
    />
}
