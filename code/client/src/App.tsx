import { Provider as PaperProvider } from 'react-native-paper';
import { NavigationContainer } from '@react-navigation/native';
import axios from 'axios';
import React from 'react';

import Conn from '@/connection';
import Auth from '@/auth';
import Nav from '@/nav';

// https://github.com/dotnet/aspnetcore/issues/38286#issuecomment-970580861
if (!(globalThis as any).document) (globalThis as any).document = undefined;

axios.defaults.baseURL = 'https://stunet-2021.herokuapp.com';

export default () => (
    <PaperProvider>
        <NavigationContainer>
            <Auth children={<Conn children={<Nav/>}/>}/>
        </NavigationContainer>
    </PaperProvider>
)
