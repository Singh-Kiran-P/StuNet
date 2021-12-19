import React, { extend, useState, useEffect, useTheme, paper, Style } from '@/.';
import { Button } from 'react-native-paper';

type Props = {
    toggled?: any;
}

export default extend<typeof Button, Props>(Button, ({ toggled, onPress, labelStyle, ...props }) => {
    let [theme] = useTheme();
    let toggle = toggled !== undefined;
    let [disabled, setDisabled] = toggle ? useState(false) : [false, () => {}];

    if (toggle) useEffect(() => {
        if (disabled) setDisabled(false);
    }, [toggled])

    const s = Style.create({
        color: {
            color: theme.bright
        }
    })

    return <Button
        mode='contained'
        onPress={() => {
            if (toggle) setDisabled(true);
            if (onPress) onPress();
        }}
        disabled={disabled}
        theme={paper(theme)}
        labelStyle={[s.color, labelStyle]}
        {...props}
    />
})
