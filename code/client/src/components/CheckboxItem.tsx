import React, { Component } from 'react';

import {
    Checkbox
} from 'react-native-paper';

type Props = {
	label: string;
    checked: () => boolean;
    oncheck: (checked: boolean) => {};
}

class CheckboxItem extends Component<Props> {
    state = {
        checked: this.props.checked()
    }

    toggle = () => (this.props.oncheck(!this.state.checked), this.setState({ checked: !this.state.checked }));
    status = () => this.state.checked ? 'checked' : 'unchecked';

    render = () => (
		<Checkbox.Item mode='ios' label={this.props.label} status={this.status()} onPress={this.toggle}>
			{this.props.children}
		</Checkbox.Item>
    )
}

export default CheckboxItem;
