import React, { extend, useState, useEffect, useTheme, paper, Style } from '@/.';
import { Button } from 'react-native-paper';

type Props = {
    toggled?: any;
}

export default extend<typeof Button, Props>(Button, ({ toggled, onPress, labelStyle, disabled, ...props }) => {
    let [theme] = useTheme();
    let toggle = toggled !== undefined;
    let [busy, setBusy] = toggle ? useState(false) : [false, () => {}];

    if (toggle) useEffect(() => {
        if (busy) setBusy(false);
    }, [toggled])

    const s = Style.create({
        color: {
            color: theme.bright
        }
    })

    return <Button
        mode='contained'
        onPress={() => {
            if (toggle) setBusy(true);
            if (onPress) onPress();
        }}
        theme={paper(theme)}
        disabled={disabled || busy}
        labelStyle={[s.color, labelStyle]}
        {...props}
    />
})
