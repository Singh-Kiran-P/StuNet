import React, { extend, useState } from '@/.';
import TextInput from '@/components/base/input/TextInput';

type Props = {
	showable?: boolean;
}

export default extend<typeof TextInput, Props>(TextInput, ({ showable = true, ...props }) => {
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
