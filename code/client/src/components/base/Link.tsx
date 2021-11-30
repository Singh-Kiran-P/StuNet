import React, { extend, theming } from '@/.';
import { Link } from '@react-navigation/native';

// TODO FIX
export default extend(Link, ({ style, ...props }) => {
    const s = theming(theme => ({
        color: theme.accent,
        textDecorationLine: 'underline'
    }))

    return <Link
        style={[s, style]}
        {...props}
    />
})
