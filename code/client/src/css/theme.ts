export const Base = {

    radius: 5,

    small: 12,
    normal: 16,
    large: 24,
    huge: 48,

    margin: 10,
    padding: 20,

    tabs: {
        auth: {
            primary: 'pink',
            accent: 'brown'
        },
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

export const Light = { ...Base,

    surface: '#e4e4e4',
    background: '#f6f6f6',
    foreground: '#000000',

    error: '#b00020',
    notification: '#f50057',

    disabled: 'rgba(0, 0, 0, 0.26)',
    placeholder: 'rgba(0, 0, 0, 0.54)'

}

export const Dark = { ...Base,

    surface: '#222222',
    background: '#141414',
    foreground: '#ffffff',

    error: '#cf6679',
    notification: '#ff80ab',

    disabled: 'rgba(255, 255, 255, 0.38)',
    placeholder: 'rgba(255, 255, 255, 0.54)'

}
