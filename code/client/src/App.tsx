import { Provider as ThemeProvider } from 'react-native-paper';
import { NavigationContainer } from '@react-navigation/native';
import { Platform, UIManager } from 'react-native';
import axios from 'axios';
import React from 'react';
import App from '@/nav';

if (Platform.OS === 'android') UIManager.setLayoutAnimationEnabledExperimental(true);

axios.defaults.baseURL = 'http://10.0.2.2:5000';

export default () => (
    <ThemeProvider>
        <NavigationContainer>
            <App/>
        </NavigationContainer>
    </ThemeProvider>
)
