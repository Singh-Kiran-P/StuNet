import React, { Component, useTheme, paper, Style } from '@/.';
import { Appbar } from 'react-native-paper';
import { SearchBar } from '@/components';
import { replace } from '@/util/alg';

export default Component(({ params, params: { screenTitle, search }, nav }) => {
    let [theme] = useTheme();
    let title = replace(screenTitle, params) || 'Loading...';

    const s = Style.create({
        search: {
            backgroundColor: theme.primary
        }
    })

    const themed = {
        primary: theme.accent,
        placeholder: theme.dimmed,
        foreground: theme.bright
    }

    return (
        <Appbar.Header theme={paper(theme)}>
            {!nav.getState().index || <Appbar.BackAction onPress={() => nav.goBack()}/>}
            {search === undefined ? <Appbar.Content title={title}/> : <SearchBar
                placeholder={title} onSearch={query => nav.setParams({ search: query })}
                iconColor={theme.bright} style={s.search} theme={paper(Object.assign({}, theme, themed))}
            />}
        </Appbar.Header>
    )
})
