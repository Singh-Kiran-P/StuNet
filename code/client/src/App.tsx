import React, { Component } from 'react';
import { Platform, UIManager } from 'react-native';
import { Provider } from 'react-native-paper';
import AskQuestion from '@pages/ask-question';
import Login from '@pages/login';
// import { Theme } from '@css';

if (Platform.OS === 'android') {
    if (UIManager.setLayoutAnimationEnabledExperimental) {
      UIManager.setLayoutAnimationEnabledExperimental(true);
    }
  }

class App extends Component {
    render = () => (
        <Provider>
            <AskQuestion/>
        </Provider>
    );
};

export default App;
