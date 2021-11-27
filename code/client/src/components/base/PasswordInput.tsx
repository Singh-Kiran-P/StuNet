import React, { Props, useState } from "@/.";
import { TextInput } from "@/components";

export default (props: Partial<Props<typeof TextInput>> & { showable: boolean }) => {
    const [passwordVisible, setPasswordVisible] = useState(false);
    const showable = props.showable;

    return <TextInput
        secureTextEntry={!passwordVisible}
        right={showable ? <TextInput.Icon
            name={passwordVisible ? 'eye' : 'eye-off'}
            onPress={() => setPasswordVisible(!passwordVisible)}
        /> : null}
        {...props as Props<typeof TextInput>}
    />
}
