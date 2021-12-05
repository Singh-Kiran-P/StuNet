import React, { extend, useTheme, paper } from '@/.';
import { Button } from 'react-native-paper';

export default extend(Button, props => {
    let [theme] = useTheme();

    return <Button
        mode='contained'
        theme={paper(theme)}
        {...props}
    />
})
