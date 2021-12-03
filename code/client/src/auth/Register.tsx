import React, { Route, useState, useToken, Style, useTheme, axios } from '@/.';

import {
	View,
	Text,
	Button,
	Loader,
	TextInput,
	PasswordInput
} from '@/components';
import { privateName } from '@babel/types';

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
	let [theme] = useTheme();

	const studentRegex = new RegExp(/\w+@student.uhasselt.be/);
	const profRegex = new RegExp(/\w+@uhasselt.be/);

	const s = Style.create({
		screen: {
			padding: theme.padding,
			backgroundColor: theme.background
        },

		header: {
			color: theme.primary,
            marginBottom: theme.padding
		},

		hint: {
            marginTop: theme.margin
        },

        margin: {
            marginBottom: theme.margin
        }
    })

	const validFOS = () : boolean => {
		if (userType != UserTypes.STUDENT) return true;
		else return !Object.values(FODSelection).some(e => !e);
	}

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
		<Loader load={getFODs} style={s.screen}>
			<TextInput style={s.margin} label='E-mail' onChangeText={validate}/>
			<PasswordInput style={s.margin} label='Password' onChangeText={setPassword} showable={false}/>
			<PasswordInput style={s.margin} label='Confirm password' onChangeText={setpasswordConfirm} showable={false}/>
			<Text style={s.margin} type='error' visible={password !== passwordConfirm}>Passwords do not match.</Text>
			{userType == UserTypes.STUDENT && <View style={[{ flexDirection: 'row' }, s.margin]}>
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
			<Text style={s.margin} type='error' visible={!!error}>{error}</Text>
			<Button style={s.margin} onPress={register} disabled={!mail || !password || password !== passwordConfirm || !validFOS()}>Register</Button>
			<Text style={s.hint} type='hint'>
				Already have an account? <Text type='link' size='small' onPress={() => navigation.navigate('Login')}>Login here!</Text>
			</Text>
		</Loader>
	)
}
