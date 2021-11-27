import { Platform, UIManager, useColorScheme } from 'react-native';
import { NavigationContainer, DarkTheme as NavigationDarkTheme, DefaultTheme as NavigationDefaultTheme, } from '@react-navigation/native';
import { Provider as ThemeProvider, DarkTheme as PaperDarkTheme, DefaultTheme as PaperDefaultTheme } from 'react-native-paper';
import axios from 'axios';
import React from 'react';

import Navigator from '@/nav';
import { Theme as customTheme } from '@/css';
  
const CombinedDefaultTheme = {...NavigationDefaultTheme, ...PaperDefaultTheme };
const CombinedDarkTheme = {...NavigationDarkTheme, ...PaperDarkTheme };

if (Platform.OS === 'android') UIManager.setLayoutAnimationEnabledExperimental(true);

axios.defaults.baseURL = 'http://10.0.2.2:5000';

export default () => {
    const scheme = useColorScheme();
    let theme = scheme === 'dark' ? CombinedDarkTheme : CombinedDefaultTheme;
    theme.colors = { ...theme.colors, ...customTheme.colors };

    return <ThemeProvider theme={theme}>
        <NavigationContainer theme={theme}>
            <Navigator />
        </NavigationContainer>
    </ThemeProvider>
}
