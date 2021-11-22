import { DefaultTheme } from 'react-native-paper';

export const Theme = {
    ...DefaultTheme,
    large: 24,
    medium: 16,
    small: 12,

    padding: 10,

    colors: {
        ...DefaultTheme.colors,

        home: {
            primary: 'blue',
            accent: 'green'
        },
        courses: {
            primary: 'green',
            accent: 'red'
        },
        notifications: {
            primary: 'red',
            accent:'purple'
        },
        profile: {
            primary: 'purple',
            accent: 'blue'
        }
    }
}
