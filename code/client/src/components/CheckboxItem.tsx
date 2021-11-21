import React, { useState } from 'react';

import {
    Checkbox
} from 'react-native-paper';

type Props = {
    label: string;
    checked: () => boolean;
    oncheck: (checked: boolean) => {};
}

export default function CheckboxItem(props: Props) {
    const [checked, setChecked] = useState(() => props.checked());

    const toggle = () => (props.oncheck(!checked), setChecked(!checked));
    const status = () => checked ? 'checked' : 'unchecked';

    return <Checkbox.Item mode='ios' label={props.label} status={status()} onPress={toggle} />
}
