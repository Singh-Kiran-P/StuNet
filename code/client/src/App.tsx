import { Provider as PaperProvider } from 'react-native-paper';
import { NavigationContainer } from '@react-navigation/native';
import { Platform, UIManager } from 'react-native';
import axios from 'axios';
import React from 'react';

import Auth from '@/auth';
import Conn from '@/connection'
import Nav from '@/nav';

// https://github.com/dotnet/aspnetcore/issues/38286#issuecomment-970580861
if (!globalThis.document) {
	(globalThis.document as any) = undefined;
}

if (Platform.OS === 'android') UIManager.setLayoutAnimationEnabledExperimental(true);

axios.defaults.baseURL = 'http://10.0.2.2:5000';

export default () => (
    <PaperProvider>
        <NavigationContainer>
            <Auth children={<Conn children={<Nav />} />} />
        </NavigationContainer>
    </PaperProvider>
)
