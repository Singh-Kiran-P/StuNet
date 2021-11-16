import React, { Component } from 'react';

import {
    StyleSheet,
    View
} from 'react-native';

import {
    Chip
} from 'react-native-paper';

class CheckboxRow extends Component {
    state = {
        selected: false
    }

	toggle = () => this.setState({ selected: !this.state.selected });

    render = () => (
		<View style={s.chip}>
			<Chip mode='outlined' selected={this.state.selected} onPress={this.toggle}>
				{this.props.children}
			</Chip>
		</View>
    )
}

const s = StyleSheet.create({
    chip: {
        margin: 2,
        flexWrap: 'wrap',
        alignContent: 'center'
    }
});

export default CheckboxRow;
