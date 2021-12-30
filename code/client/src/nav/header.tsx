import React, { Component, useToken, useTheme, paper, Style } from '@/.';
import { Appbar } from 'react-native-paper';
import { SearchBar } from '@/components';
import { replace } from '@/util/alg';

export default Component(({ nav, params, params: { screenTitle, search, subscribe, logout } }) => {
    let [_, setToken] = logout ? useToken() : ['', () => {}];
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
                style={s.search} theme={paper(Object.assign({}, theme, themed))}
                iconColor='bright' returnKeyType='search' placeholder={title}
                onSearch={query => nav.setParams({ search: query })}
            />}
            {subscribe !== undefined && <Appbar.Action
                icon={subscribe ? 'bell-ring' : 'bell-outline'}
                onPress={() => nav.setParams({ subscribe: !subscribe })}
            />}
            {logout && <Appbar.Action
                icon='logout'
                onPress={() => setToken('')}
            />}
        </Appbar.Header>
    )
})
