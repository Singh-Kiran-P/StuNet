import React, { axios, Screen, useState } from "@/.";
import { Button, LoadingWrapper, PasswordInput, Text, TextInput, View } from "@/components";
import { Picker } from '@react-native-picker/picker';
import { HelperText } from "react-native-paper";

type FieldApi = {
	id:number,
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

const enum userTypes {
	UNKNOWN,
	STUDENT,
	PROFESSOR
}

export default Screen('Register', ({ params, nav }) => {
	const [mail, setMail] = useState('');
	const [userType, setUserType] = useState<userTypes>(userTypes.UNKNOWN);
    const [password, setPassword] = useState('');
    const [passwordConfirm, setpasswordConfirm] = useState('');
	const [fields, setFields] = useState<Fields>({});
 	const [FODSelection, setFODSelection] = useState<FODSelection>({name: '', degree: '', year: ''});
	const [errMessage, setErrMessage] = useState('');

	const studentRegex = new RegExp(/\w+@student.uhasselt.be/);
	const profRegex = new RegExp(/\w+@uhasselt.be/);

	const getFODs = async () => {
		return axios.get('/FieldOfStudy')
			.then(res => {
				var ret: Fields = {};
				res.data.forEach((element: FieldApi) => {
					if (!(element.name in ret)) {
						ret[element.name] = { Bachelor: [], Master: [] }
					}

					var years: number[] = [];
					if (element.isBachelor) years = ret[element.name].Bachelor;
					else years = ret[element.name].Master;

					years.push(element.year);
				});

				console.log(ret);

				setFields(ret);
			})
			.catch(err => console.log(err));
	}

	const validate = (s: string) => {
		setMail(s);

		if (studentRegex.test(s)) {
			setUserType(userTypes.STUDENT);
		} else if (profRegex.test(s)) {
			setUserType(userTypes.PROFESSOR);
		} else {
			setUserType(0);
		}
	}

	const register = () => {

		var degree = FODSelection.degree == 'Bachelor' ? 'BACH' : 'MASTER';

        axios.post('/Auth/register', {
            Email: mail,
            Password: password,
			ConfirmPassword: passwordConfirm,
			FieldOfStudy: FODSelection.name + '-' + degree + '-' + FODSelection.year //ex. Informatica-BACH-3
        })
        .then(res => nav.replace('Login'))
        .catch(err => { setErrMessage(err.response.data) });
    }


	return (
		<LoadingWrapper func={getFODs}>
			<TextInput label='E-mail' onChangeText={validate}/>
            <PasswordInput label='Password' onChangeText={setPassword} showable={false} />
			<PasswordInput label='Confirm password' onChangeText={setpasswordConfirm} showable={false} />
			<HelperText type='error' visible={password !== passwordConfirm}>
				Passwords do not match.
			</HelperText>
			{userType == userTypes.STUDENT ?
				<View style={{ flexDirection: 'row' }}>
				<Picker prompt={'Degree'} mode='dropdown' style={{ flex: 1 }} selectedValue={FODSelection.name} onValueChange={(value: string, _) => setFODSelection({ ...FODSelection, name: value })}>
					<Picker.Item label='Field' value='' enabled={false} />
					{Object.keys(fields).map((name, i) => {
						return <Picker.Item key={i} label={name} value={name} />
					})}
				</Picker>
				<Picker prompt={'Field'} mode='dropdown' style={{ flex: 1 }} selectedValue={FODSelection.degree} onValueChange={(value: string, _) => setFODSelection({ ...FODSelection, degree: value })} enabled={FODSelection.name != ''}>
					<Picker.Item label='Degree' value='' enabled={false} />
					{FODSelection.name !== '' ? Object.keys(fields[FODSelection.name]).map((degrees, i) => {
						return <Picker.Item key={i} label={degrees} value={degrees} />
					}) : <></>}
					</Picker>
				<Picker prompt={'Year'} mode='dropdown' style={{ flex: 1 }} selectedValue={FODSelection.year} onValueChange={(value: string, _) => setFODSelection({ ...FODSelection, year: value })} enabled={FODSelection.degree != ''}>
				<Picker.Item label='Year' value='' enabled={false} />
					{FODSelection.degree !== '' ? fields[FODSelection.name][FODSelection.degree].map((years, i) => {
							return <Picker.Item key={i} label={years.toString()} value={years} />
						}) : <></>}
					</Picker>
				</View>
				: <></>}
			
            <HelperText type='error' visible={errMessage !== ''}>{errMessage}</HelperText>
			<Button onPress={register} disabled={!mail || !password || password !== passwordConfirm || Object.values(FODSelection).some(e => e === '')}>Register</Button>
		</LoadingWrapper>
	)
})