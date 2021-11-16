import React, { Component } from 'react';

import {
    Checkbox
} from 'react-native-paper';

type Props = {
	label: string;
}

export default class CheckboxItem extends Component<Props> {
    state = {
        checked: false,
    }

    constructor(props: Props) {
        super(props);
    }

    toggle = () => this.setState({ checked: !this.state.checked });
    status = () => this.state.checked ? 'checked' : 'unchecked';

    render = () => (
		<Checkbox.Item mode='ios' label={this.props.label} status={this.status()} onPress={this.toggle}>
			{this.props.children}
		</Checkbox.Item>
    )
}
