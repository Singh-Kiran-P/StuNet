import React, { Component } from 'react';
import { Platform, UIManager } from 'react-native';
import { Provider } from 'react-native-paper';

import Login from '@pages/login';
import AskQuestion from '@pages/ask-question';

if (Platform.OS === 'android' && UIManager.setLayoutAnimationEnabledExperimental)
    UIManager.setLayoutAnimationEnabledExperimental(true);

class App extends Component {
    render = () => (
        <Provider>
            <AskQuestion />
        </Provider>
    )
}

export default App;
