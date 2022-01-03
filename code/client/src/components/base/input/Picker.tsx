import React, { extend, useTheme } from '@/.';
import { Picker, PickerProps } from '@react-native-picker/picker';

type Props = {
    values?: string[];
}

type ValueProps = Omit<PickerProps<string>, keyof Props> & Props;

export default extend<typeof Picker, ValueProps>(Picker, ({ values, prompt, children, ...props }) => {
    let [theme] = useTheme();

    return <Picker
        mode='dropdown'
        dropdownIconColor={theme.accent}
        itemStyle={{ color: theme.foreground }}
        {...props}
    >
        <Picker.Item label={prompt} value='' enabled={false}/>
        {values?.map((name, i) => <Picker.Item key={i} label={name} value={name}/>) || null}
        {children}
    </Picker>
})
