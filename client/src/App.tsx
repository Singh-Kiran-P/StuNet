import React, { Component } from 'react';
import { Provider, DefaultTheme } from 'react-native-paper';
import Login from 'pages/login';

class App extends Component {
    render = () => (
        <Provider>
            <Login />
        </Provider>
    );
};

export default App;
