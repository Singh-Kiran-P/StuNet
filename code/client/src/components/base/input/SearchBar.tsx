import React, { extend, useState, useTheme, paper, Color } from '@/.';
import { Searchbar } from 'react-native-paper';

type Props = {
    onSearch: (query: string, setQuery: (query: string) => void) => void;
    disableEmpty?: boolean;
    disabledColor?: Color;
    iconColor?: Color;
}

export default extend<typeof Searchbar, Props>(Searchbar, ({ onSearch, value, disableEmpty, disabledColor, iconColor, ...props }) => {
    let [theme] = useTheme();
    let [query, setQuery] = useState('');
    let disabled = disableEmpty && ! query;

    return <Searchbar
        theme={paper(theme)}
        value={value || query}
        onChangeText={setQuery}
        onIconPress={disabled ? undefined : () => onSearch(query, setQuery)}
        onSubmitEditing={disabled ? undefined : () => onSearch(query, setQuery)}
        iconColor={disabled ? theme[disabledColor || 'disabled'] : theme[iconColor || 'foreground']}
        {...props}
    />
})
