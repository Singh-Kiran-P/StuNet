import React from 'react';
import axios from 'axios'
import { Platform, UIManager } from 'react-native';
import { Provider } from 'react-native-paper';

axios.defaults.baseURL = 'http://10.0.2.2:5000'

import Login from '@pages/login';
import AskQuestion from '@pages/ask-question';

if (Platform.OS === 'android' && UIManager.setLayoutAnimationEnabledExperimental)
    UIManager.setLayoutAnimationEnabledExperimental(true);

function App() {
    return (
        <Provider>
            <AskQuestion />
        </Provider>
    )
}

export default App;
