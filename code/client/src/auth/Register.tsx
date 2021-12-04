import React, { Route, Style, Field, FOS, User, useTheme, useState, axios, errorString } from '@/.';
import { View, Text, Button, Loader, Picker, TextInput, PasswordInput } from '@/components';

type Fields = { [name: string]: { [degree: string]: number[] } };

const profRegex = new RegExp(/\w+@uhasselt\.be/);
const studentRegex = new RegExp(/\w+@student\.uhasselt\.be/);

export default ({ navigation }: Route) => {
	const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
	const [FOS, setFOS] = useState<FOS>({ field: '', degree: '', year: '' });
	const [fields, setFields] = useState<Fields>({});
	const [error, setError] = useState('');
	let [theme] = useTheme();

	const s = Style.create({
		screen: {
			flex: 1,
			padding: theme.padding,
			backgroundColor: theme.background
        },

		header: {
			color: theme.primary,
            marginBottom: theme.padding
		},

		FOS: {
			flexDirection: 'row'
		},

		picker: {
			flex: 1
		}
    })

	const type = () => {
		if (profRegex.test(email)) return User.PROF;
		if (studentRegex.test(email)) return User.STUDENT;
		return null;
	}

	const degrees = (field: Fields[string]) => Object.keys(field || []).filter(degree => Object.values(field[degree]).length);
	const years = (field: Fields[string], degree: string) => ((field || {})[degree] || []).map(degree => degree.toString());

	const fetch = async () => {
		return axios.get('/FieldOfStudy').then(res => {
			setFields((res.data as Field[]).reduce((acc, cur) => ({ ...acc, [cur.name]: (o => {
					return (o[Object.keys(o)[cur.isBachelor ? 0  : 1]].push(cur.year), o);
				})(acc[cur.name] || { Bachelor: [], Master: [] })
			}), {} as Fields));
		})
	}

	const register = () => {
        axios.post('/Auth/register', {
            Email: email,
            Password: password,
			ConfirmPassword: confirmPassword,
			FieldOfStudy: `${FOS.field}-${FOS.degree}-${FOS.year}`
        }).then(res => {}) // TODO info about email confirmation
        .catch(err => setError(errorString(err)));
    }

	return (
		<Loader load={fetch} style={s.screen}>
			<Text style={s.header} type='header' children='Register'/>

			<TextInput label='Email' onChangeText={setEmail}/>
			<PasswordInput margin label='Password' onChangeText={setPassword}/>
			<PasswordInput margin label='Confirm password' onChangeText={setConfirmPassword}/>
			<Text margin type='error' hidden={password == confirmPassword} children='Passwords do not match.'/>

			<View margin style={s.FOS} hidden={type() != User.STUDENT}>
				<Picker prompt='Field' style={s.picker}
					selectedValue={FOS.field} values={Object.keys(fields)}
					onValueChange={v => setFOS({ field: v, degree: '', year: '' })}/>

				<Picker prompt='Degree' style={s.picker} enabled={!!FOS.field}
					selectedValue={FOS.degree} values={degrees(fields[FOS.field])}
					onValueChange={v => setFOS({ ...FOS, degree: v, year: '' })}/>
	
				<Picker prompt='Year' style={s.picker} enabled={!!FOS.degree}
					selectedValue={FOS.year} values={years(fields[FOS.field], FOS.degree)}
					onValueChange={v => setFOS({ ...FOS, year: v })}/>
			</View>

			<Text margin type='error' hidden={!error} children={error}/>
			<Button margin onPress={register} disabled={!email || !password || password !== confirmPassword || !type()} children='Register'/>
			<Text margin type='hint'>
				Already have an account?{' '}
				<Text type='link' size='auto' onPress={() => navigation.navigate('Login')}>
					Log in here!
				</Text>
			</Text>
		</Loader>
	)
}
