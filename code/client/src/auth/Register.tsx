import React, { Route, Style, Field, useTheme, useState, axios, show } from '@/.';
import { View, Text, Button, Loader, Picker, TextInput, PasswordInput } from '@/components';

type Fields = { [name: string]: [number, number] };

const enum User {
	PROF,
	STUDENT
}

const profRegex = new RegExp(/\w+@uhasselt\.be/);
const studentRegex = new RegExp(/\w+@student\.uhasselt\.be/);

export default ({ navigation }: Route) => {
	let [email, setEmail] = useState('');
    let [password, setPassword] = useState('');
    let [confirmPassword, setConfirmPassword] = useState('');
	let [FOS, setFOS] = useState<[string, number, string]>(['', NaN, '']);
	let [fields, setFields] = useState<Fields>({});
	let [error, setError] = useState('');
	let [theme] = useTheme();

	const s = Style.create({
		screen: {
			backgroundColor: theme.background
        }
    })

	const type = () => {
		if (profRegex.test(email)) return User.PROF;
		if (studentRegex.test(email)) return User.STUDENT;
		return null;
	}

	const study = () => (fields[FOS[0]] || [])[FOS[1]];

	const valid = () => {
		let t = type();
		if (t === null) return false;
		if (t === User.PROF) return true;
		let s = study();
		return !!s;
	}

	const degrees = (field?: Fields[string]) => (field || []).map((f, i) => !f ? '' : !i ? 'Bachelor' : 'Master').filter(d => d);

	const fetch = async () => {
		return axios.get('/FieldOfStudy').then(res => {
			setFields((res.data as Field[]).reduce((acc, cur) => ({ ...acc, [cur.name]: (o => {
					return (o[cur.isBachelor ? 0 : 1] = cur.id, o);
				})(acc[cur.name] || [NaN, NaN])
			}), {} as Fields));
		})
	}

	const register = () => {
		setError('');
        axios.post('/Auth/register', {
            Email: email,
            Password: password,
			ConfirmPassword: confirmPassword,
			FieldOfStudy: study()
        }).then(() => navigation.navigate('Login', { registered: email }), show(setError))
    }

	return (
		<Loader style={s.screen} padding load={fetch}>
			<Text type='title' children='Register'/>
			<TextInput label='Email' onChangeText={setEmail}/>
			<PasswordInput margin label='Password' onChangeText={setPassword}/>
			<PasswordInput margin label='Confirm password' onChangeText={setConfirmPassword}/>
			<Text type='error' margin hidden={password == confirmPassword} children='Passwords do not match.'/>
			<View type='row' margin hidden={type() != User.STUDENT}>
				<Picker flex prompt='Field'
					selectedValue={FOS[0]} values={Object.keys(fields)}
					onValueChange={v => setFOS([v, NaN, ''])}/>
				<Picker flex prompt='Degree' enabled={!!FOS[0]}
					selectedValue={FOS[2]} values={degrees(fields[FOS[0]])}
					onValueChange={(v, i) => setFOS([FOS[0], i - 1, v])}/>
			</View>
			<Text type='error' margin hidden={!error} children={error}/>
			<Button margin children='Register' disabled={(password || NaN) !== confirmPassword || !valid()} toggled={error} onPress={register}/>
			<Text type='hint' margin>
				Already have an account?{' '}
				<Text type='link' size='auto' onPress={() => navigation.navigate('Login')}>
					Log in here!
				</Text>
			</Text>
		</Loader>
	)
}
