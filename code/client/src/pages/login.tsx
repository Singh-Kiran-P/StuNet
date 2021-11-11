import React, { Component } from 'react';

import {
    TouchableRipple,
    TextInput,
    Button
} from 'react-native-paper';


import {
    StyleSheet,
    View,
    Alert
} from 'react-native';

class Test extends Component {
    state = {
        passwordVisible: false,
        email: '',
        password: '',
    }

    togglePasswordVisible() {
        this.setState({passwordVisible: !this.state.passwordVisible});
    }

    submit() { // For testing purposes
        Alert.alert(
            "Submit received",
            "email: " + this.state.email + "\n" +
            "password: " + this.state.password);
    }

    render = () => (
        <View style={[s.view]}>
            <TextInput style={[s.input]} label="E-mail" mode="outlined" onChangeText={s => this.setState({email: s})}/>
            <TextInput style={[s.input]} label="Password" mode="outlined" onChangeText={s => this.setState({password: s})}
                secureTextEntry={!this.state.passwordVisible} 
                right={<TextInput.Icon name={this.state.passwordVisible ? "eye" : "eye-off"} 
                    onPress={() => this.togglePasswordVisible() }/>}/>
            <TouchableRipple style={s.button}>
                <Button mode="contained" disabled={this.state.email == '' || this.state.password == ''} onPress={() => this.submit()}>Login</Button>
            </TouchableRipple>
        </View>
    );
};

const padding = 10;
const s = StyleSheet.create({
    view: {
        padding: padding,
    },

    input: {
        marginBottom: padding
    },

    button: {
        width: '30%',
        alignSelf: 'flex-end'
    }
});

export default Test;
