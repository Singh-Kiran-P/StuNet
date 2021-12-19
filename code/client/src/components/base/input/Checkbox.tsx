import React, { extend, useTheme, paper } from '@/.';
import { Checkbox } from 'react-native-paper';

type Props = {
    checked?: boolean;
}

export default extend<typeof Checkbox.Item, Props>(Checkbox.Item, ({ checked, status, ...props }) => {
    let [theme] = useTheme();

    return <Checkbox.Item
        mode='ios'
        theme={paper(theme)}
        status={status || (checked === false ? 'unchecked' : checked ? 'checked' : 'indeterminate')}
        {...props}
    />
})
