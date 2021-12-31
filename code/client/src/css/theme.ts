const Sizes = {
    small: 12,
    normal: 16,
    bigger: 20,
    large: 24,
    huge: 36,
    massive: 48
}

export type Size = keyof typeof Sizes;

export const Base = {
    ...Sizes,

    radius: 5,

    margin: 10,
    padding: 20,

    tabs: {
        auth: {
            primary: '#C0CA33',
            accent: '#E53935'
        },
        home: {
            primary: '#01579B',
            accent: '#9575CD'
        },
        courses: {
            primary: '#1B5E20',
            accent: '#F57F17'
        },
        notifications: {
            primary: '#BF360C',
            accent:'#F06292'
        },
        profile: {
            primary: '#004D40',
            accent: '#6D4C41'
        }
    },

    bright: '#ffffff',
    dimmed: 'rgba(255, 255, 255, 0.54)',

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

type Base = typeof Base;
export type Color = Exclude<keyof (typeof Light & typeof Dark), keyof typeof Base> | 'primary' | 'accent' | 'bright' | 'dimmed';
