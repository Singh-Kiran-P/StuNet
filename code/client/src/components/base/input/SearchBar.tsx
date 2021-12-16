import React, { extend, useState, useTheme, paper } from '@/.';
import { Searchbar } from 'react-native-paper';

type Props = {
    onSearch: (query: string) => void;
}

export default extend<typeof Searchbar, Props>(Searchbar, ({ onSearch, ...props }) => {
    let [theme] = useTheme();
    let [query, setQuery] = useState('');

    return <Searchbar
        theme={paper(theme)}
        onChangeText={setQuery}
        onIconPress={() => onSearch(query)}
        onSubmitEditing={() => onSearch(query)}
        {...props}
    />
})
