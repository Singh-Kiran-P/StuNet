import React, { Props, Style, Theme } from '@/.';
import { Text } from 'react-native-paper';

type Mode = {
    mode: keyof typeof style;
}

export default (props: Partial<Props<typeof Text> & Mode>) => {
    return <Text
        {...props as Props<typeof Text>}
        style={[style[props.mode || 'normal'], props.style]}
    />
}

const style = Style.create({

    normal: {

    },

    header: {
        fontWeight: 'bold',
        fontSize: Theme.large
    }

})
