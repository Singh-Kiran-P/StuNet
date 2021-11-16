import React from 'react';
import { Platform, UIManager } from 'react-native';
import { Provider } from 'react-native-paper';

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
