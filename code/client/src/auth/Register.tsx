import React, { Route, useState, useToken, Style, Theme, axios } from '@/.';

import {
	View,
	Text,
	Button,
	Loader,
	TextInput,
	PasswordInput
} from '@/components';

import { Picker } from '@react-native-picker/picker';

type FieldApi = {
	id: number,
	fullName: string,
	name: string,
	isBachelor: boolean,
	year: number
}

type Fields = Record<string, Record<string, number[]>>

type FODSelection = {
	name: string,
	degree: string,
	year: string
}

const enum UserTypes {
	UNKNOWN,
	STUDENT,
	PROFESSOR
}

export default ({ navigation }: Route) => {
	const [mail, setMail] = useState('');
    const [password, setPassword] = useState('');
	const [fields, setFields] = useState<Fields>({});
    const [passwordConfirm, setpasswordConfirm] = useState('');
	const [userType, setUserType] = useState<UserTypes>(UserTypes.UNKNOWN);
 	const [FODSelection, setFODSelection] = useState<FODSelection>({ name: '', degree: '', year: '' });
	const [error, setError] = useState('');
	let [_, setToken] = useToken();

	const studentRegex = new RegExp(/\w+@student.uhasselt.be/);
	const profRegex = new RegExp(/\w+@uhasselt.be/);

	const s = Style.create({
        screen: {
            padding: Theme.padding
        },

        header: {
            marginBottom: Theme.padding
        },

        margin: {
            marginBottom: Theme.margin
        }
    })

	const getFODs = async () => {
		return axios.get('/FieldOfStudy')
			.then(res => {
				let ret: Fields = {};
				res.data.forEach((element: FieldApi) => {
					if (!(element.name in ret)) {
						ret[element.name] = { Bachelor: [], Master: [] }
					}

					let years: number[] = [];
					if (element.isBachelor) years = ret[element.name].Bachelor;
					else years = ret[element.name].Master;

					years.push(element.year);
				});
				setFields(ret);
			})
	}

	const validate = (s: string) => {
		setMail(s);

		if (studentRegex.test(s)) setUserType(UserTypes.STUDENT);
		else if (profRegex.test(s)) setUserType(UserTypes.PROFESSOR);
		else setUserType(0);
	}

	const register = () => {
		let degree = FODSelection.degree == 'Bachelor' ? 'BACH' : 'MASTER';

        axios.post('/Auth/register', {
            Email: mail,
            Password: password,
			ConfirmPassword: passwordConfirm,
			FieldOfStudy: FODSelection.name + '-' + degree + '-' + FODSelection.year
        })
        .then(res => setToken(res.data))
        .catch(err => setError(err.response.data));
    }

	return (
		<Loader load={getFODs}>
			<View style={s.screen}>
				<TextInput label='E-mail' onChangeText={validate}/>
				<PasswordInput label='Password' onChangeText={setPassword} showable={false}/>
				<PasswordInput label='Confirm password' onChangeText={setpasswordConfirm} showable={false}/>
				<Text type='error' visible={password !== passwordConfirm}>Passwords do not match.</Text>
				{userType == UserTypes.STUDENT && <View style={{ flexDirection: 'row' }}>
					<Picker prompt='Degree' mode='dropdown' style={{ flex: 1 }} selectedValue={FODSelection.name} onValueChange={value => setFODSelection({ name: value, degree: '', year: '' })}>
						<Picker.Item label='Field' value='' enabled={false} />
						{Object.keys(fields).map((name, i) => (
							<Picker.Item key={i} label={name} value={name} />
						))}
					</Picker>
					<Picker prompt='Field' mode='dropdown' style={{ flex: 1 }} selectedValue={FODSelection.degree} onValueChange={value => setFODSelection({ ...FODSelection, degree: value, year: '' })} enabled={!!FODSelection.name}>
						<Picker.Item label='Degree' value='' enabled={false} />
						{!FODSelection.name ? null : Object.keys(fields[FODSelection.name]).map((degrees, i) => (
							<Picker.Item key={i} label={degrees} value={degrees} />
						))}
					</Picker>
					<Picker prompt='Year' mode='dropdown' style={{ flex: 1 }} selectedValue={FODSelection.year} onValueChange={value => setFODSelection({ ...FODSelection, year: value })} enabled={!!FODSelection.degree}>
					<Picker.Item label='Year' value='' enabled={false} />
						{!FODSelection.degree ? null : fields[FODSelection.name][FODSelection.degree].map((years, i) => (
							<Picker.Item key={i} label={years.toString()} value={years} />
						))}
					</Picker>
				</View>}
				<Text type='error' visible={!!error}>{error}</Text>
				<Button onPress={register} disabled={!mail || !password || password !== passwordConfirm || Object.values(FODSelection).some(e => !e)}>Register</Button>
				<Button onPress={() => navigation.push('Login')}>Login</Button>
			</View>
		</Loader>
	)
}
