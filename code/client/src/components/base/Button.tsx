import React, { extend } from '@/.';
import { Button } from 'react-native-paper';

export default extend(Button, props => {
    return <Button
        mode='contained'
        {...props}
    />
})
