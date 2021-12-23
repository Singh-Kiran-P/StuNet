import React, { Route, Style, Field, FOS, User, useTheme, useState, axios, show } from '@/.';
import { View, Text, Button, Loader, Picker, TextInput, PasswordInput } from '@/components';

type Fields = { [name: string]: { [degree: string]: number[] } };

const profRegex = new RegExp(/\w+@uhasselt\.be/);
const studentRegex = new RegExp(/\w+@student\.uhasselt\.be/);

export default ({ navigation }: Route) => {
	const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
	const [FOS, setFOS] = useState<FOS>({ field: '', degree: '' });
	const [fields, setFields] = useState<Fields>({});
	const [error, setError] = useState('');
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

	const degrees = (field: Fields[string]) => Object.keys(field || []).filter(degree => Object.values(field[degree]).length);

	const fetch = async () => {
		return axios.get('/FieldOfStudy').then(res => {
			setFields((res.data as Field[]).reduce((acc, cur) => ({ ...acc, [cur.name]: (o => {
					return (o[Object.keys(o)[cur.isBachelor ? 0  : 1]].push(-1), o);
				})(acc[cur.name] || { Bachelor: [], Master: [] })
			}), {} as Fields));
		})
	}

	const register = () => {
		let degree = FOS.degree === 'Bachelor' ? 'BACH' : 'MAST';

        axios.post('/Auth/register', {
            Email: email,
            Password: password,
			ConfirmPassword: confirmPassword,
			FieldOfStudy: `${FOS.field}-${degree}`
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
					selectedValue={FOS.field} values={Object.keys(fields)}
					onValueChange={v => setFOS({ field: v, degree: '' })}/>

				<Picker  flex prompt='Degree' enabled={!!FOS.field}
					selectedValue={FOS.degree} values={degrees(fields[FOS.field])}
					onValueChange={v => setFOS({ ...FOS, degree: v })}/>

			</View>

			<Text type='error' margin hidden={!error} children={error}/>
			<Button margin onPress={register} disabled={!email || !password || password !== confirmPassword || !type()} children='Register'/>
			<Text type='hint' margin>
				Already have an account?{' '}
				<Text type='link' size='auto' onPress={() => navigation.navigate('Login')}>
					Log in here!
				</Text>
			</Text>
		</Loader>
	)
}
