import React, { Component } from 'react';

import {
    DefaultTheme as Theme,
    TouchableRipple,
    TextInput
} from 'react-native-paper';


import {
    Text,
    StyleSheet,
    View
} from 'react-native';

class Test extends Component {
    email = '';
    password = '';

    render = () => (
        <View style={[s.view]}>
            <TextInput style={[s.input]} label="E-mail" mode="outlined" />
            <TextInput style={[s.input]} label="Password" mode="outlined" />
            <TouchableRipple style={[s.button]} onPress={() => {}}>
                <Text style={[s.buttonText]}>Login</Text>
            </TouchableRipple>
        </View>
    );
};

const padding = 10;
const s = StyleSheet.create({
    view: {
        padding: padding,
        backgroundColor: Theme.colors.background
    },

    input: {
        marginBottom: padding
    },
    
    button: {
        padding: padding,
        backgroundColor: Theme.colors.primary,
        borderRadius: Theme.roundness
    },
    
    buttonText: {
        color: Theme.colors.background,
        textAlign: 'center'
    }
});

export default Test;
