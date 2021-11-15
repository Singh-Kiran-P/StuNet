import React, { Component } from 'react';

import {
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
        fetch('http://10.0.2.2:5000/User', {
            method: 'POST',
            headers: {
                Accept: '*/*',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                email: this.state.email,
                password: this.state.password
            })
        }).then(() => Alert.alert("Succes"))
        .catch((response) => Alert.alert("Error", response))
    }

    render = () => (
        <View style={[s.view]}>
            <TextInput style={[s.input]} label="E-mail" mode="outlined" onChangeText={s => this.setState({email: s})}/>
            <TextInput style={[s.input]} label="Password" mode="outlined" onChangeText={s => this.setState({password: s})}
                secureTextEntry={!this.state.passwordVisible} 
                right={<TextInput.Icon name={this.state.passwordVisible ? "eye" : "eye-off"} 
                    onPress={() => this.togglePasswordVisible() }/>}/>
            <Button style={s.button} mode="contained" disabled={this.state.email == '' || this.state.password == ''} onPress={() => this.submit()}>Login</Button>
        </View>
    );
};

const padding = 20;
const s = StyleSheet.create({
    view: {
        padding: padding,
    },

    input: {
        marginBottom: padding / 2
    },

    button: {
        width: '30%',
        alignSelf: 'flex-end'
    }
});

export default Test;
