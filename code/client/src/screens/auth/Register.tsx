import React, { axios, Screen, useState } from '@/.';
import { Button, Loader, PasswordInput, Text, TextInput, View } from '@/components';
import { Picker } from '@react-native-picker/picker';
import { HelperText } from 'react-native-paper';

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

export default Screen('Register', ({ params, nav }) => {
	const [mail, setMail] = useState('');
	const [userType, setUserType] = useState<UserTypes>(UserTypes.UNKNOWN);
    const [password, setPassword] = useState('');
    const [passwordConfirm, setpasswordConfirm] = useState('');
	const [fields, setFields] = useState<Fields>({});
 	const [FODSelection, setFODSelection] = useState<FODSelection>({ name: '', degree: '', year: '' });
	const [errMessage, setErrMessage] = useState('');

	const studentRegex = new RegExp(/\w+@student.uhasselt.be/);
	const profRegex = new RegExp(/\w+@uhasselt.be/);

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
			.catch(err => {}); // TODO handle error
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
        .then(res => nav.replace('Login'))
        .catch(err => { setErrMessage(err.response.data) });
    }


	return (
		<Loader load={getFODs}>
			<TextInput label='E-mail' onChangeText={validate}/>
            <PasswordInput label='Password' onChangeText={setPassword} showable={false}/>
			<PasswordInput label='Confirm password' onChangeText={setpasswordConfirm} showable={false}/>
			<HelperText type='error' visible={password !== passwordConfirm}>Passwords do not match.</HelperText>
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
            <HelperText type='error' visible={!!errMessage}>{errMessage}</HelperText>
			<Button onPress={register} disabled={!mail || !password || password !== passwordConfirm || Object.values(FODSelection).some(e => !e)}>Register</Button>
		</Loader>
	)
})