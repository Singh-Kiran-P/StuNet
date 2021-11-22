export default {}; // TODO

// old login:

/*

import React, { Component } from 'react';

import { Theme } from '@/css';

import {
    View,
    Alert,
    StyleSheet
} from 'react-native';

import {
    Button,
    TextInput
} from 'react-native-paper';

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
            body: JSON.stringify({
                email: this.state.email,
                password: this.state.password
            })
        }).then(() => Alert.alert('Succes'))
        .catch(err => Alert.alert('Error', err))
    }

    render = () => (
        <View style={s.view}>
            <TextInput style={s.input} label='E-mail' mode='outlined' onChangeText={s => this.setState({email: s})}/>
            <TextInput style={s.input} label='Password' mode='outlined'
                onChangeText={s => this.setState({password: s})}
                secureTextEntry={!this.state.passwordVisible}
                right={<TextInput.Icon
                    name={this.state.passwordVisible ? 'eye' : 'eye-off'}
                    onPress={() => this.togglePasswordVisible()}
                />}
            />
            <Button style={s.button} mode='contained' disabled={!this.state.email || !this.state.password} onPress={() => this.submit()}>Login</Button>
        </View>
    )
}

const s = StyleSheet.create({
    view: {
        padding: Theme.padding,
    },

    input: {
        marginBottom: Theme.padding / 2
    },

    button: {
        width: '30%',
        alignSelf: 'flex-end'
    }
});

export default Test;

*/
