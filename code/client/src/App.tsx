import React from 'react';

import { NavigationContainer } from '@react-navigation/native';
import { Provider as ThemeProvider } from 'react-native-paper';
import { Platform, UIManager } from 'react-native';
import Navigator from '@/nav';

if (Platform.OS === 'android' && UIManager.setLayoutAnimationEnabledExperimental)
    UIManager.setLayoutAnimationEnabledExperimental(true);

const App = () => (
    <NavigationContainer>
        <ThemeProvider>
            <Navigator/>
        </ThemeProvider>
    </NavigationContainer>
)

export default App;
