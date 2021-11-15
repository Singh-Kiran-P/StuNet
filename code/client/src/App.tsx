import React, { Component } from 'react';
import { Provider } from 'react-native-paper';
import AskQuestion from '@pages/ask-question';
import { Theme } from '@css';

class App extends Component {
    render = () => (
        <Provider theme={Theme}>
            <AskQuestion />
        </Provider>
    );
};

export default App;
