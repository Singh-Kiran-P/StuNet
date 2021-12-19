import React, { extend, useState, useTheme, paper } from '@/.';
import { Searchbar } from 'react-native-paper';

type Props = {
    onSearch: (query: string, setQuery: (query: string) => void) => void;
    disableEmpty?: boolean;
}

export default extend<typeof Searchbar, Props>(Searchbar, ({ onSearch, value, disableEmpty, ...props }) => {
    let [theme] = useTheme();
    let [query, setQuery] = useState('');
    let disabled = disableEmpty && ! query;

    return <Searchbar
        theme={paper(theme)}
        value={value || query}
        onChangeText={setQuery}
        iconColor={disabled ? theme.dimmed : theme.bright}
        onIconPress={disabled ? undefined : () => onSearch(query, setQuery)}
        onSubmitEditing={disabled ? undefined : () => onSearch(query, setQuery)}
        {...props}
    />
})
