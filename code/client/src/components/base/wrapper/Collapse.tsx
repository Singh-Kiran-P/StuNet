import React, { extend, animate, useTheme, paper } from '@/.';
import { List } from 'react-native-paper';

export default extend(List.Accordion, props => {
    let [theme] = useTheme();

    return <List.Accordion
        onPress={animate}
        theme={paper(theme)}
        {...props}
    />
})
