import React, { extend, useState } from '@/.';
import { TextInput } from '@/components';

type Props = {
	showable: boolean
}

export default extend<typeof TextInput, Props>(TextInput, ({ showable, ...props }) => {
    const [visible, setVisible] = useState(false);

	return <TextInput
		secureTextEntry={!visible}
		right={showable && <TextInput.Icon
			name={visible ? 'eye' : 'eye-off'}
			onPress={() => setVisible(!visible)}
		/>}
		{...props}
	/>
})
