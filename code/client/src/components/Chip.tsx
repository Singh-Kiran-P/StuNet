import React, { Component } from 'react';
import { View } from 'react-native'
import { Chip } from 'react-native-paper'

export default class CheckboxRow extends Component {
    state = {
        selected: false,
    }


	toggle() {
        this.setState({
            selected: !this.state.selected
        })
    }

    render = () => (
		<View style={{flexWrap: 'wrap', margin: 2, alignContent: 'center'}}>
			<Chip mode='outlined' selected={this.state.selected} onPress={() => this.toggle()}>
				{this.props.children}
			</Chip>
		</View>
    )
}
