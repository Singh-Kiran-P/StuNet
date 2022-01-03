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
            primary: '#607d8b',
            accent: '#ffc107'
        },
        home: {
            primary: '#01579b',
            accent: '#9575cd'
        },
        courses: {
            primary: '#1b5e20',
            accent: '#f57c00'
        },
        notifications: {
            primary: '#bf360c',
            accent:'#f06292'
        },
        profile: {
            primary: '#006064',
            accent: '#795548'
        }
    },

    error: '#b71c1c',
    bright: '#f6f6f6',
    dimmed: 'rgba(255, 255, 255, 0.54)'

}

export const Light = { ...Base,

    surface: '#e4e4e4',
    background: '#f6f6f6',
    foreground: '#141414',

    disabled: 'rgba(0, 0, 0, 0.26)',
    placeholder: 'rgba(0, 0, 0, 0.54)'

}

export const Dark = { ...Base,

    surface: '#222222',
    background: '#141414',
    foreground: '#f6f6f6',

    disabled: 'rgba(255, 255, 255, 0.38)',
    placeholder: 'rgba(255, 255, 255, 0.54)'

}

type Base = typeof Base;
export type Color = Exclude<keyof (typeof Light & typeof Dark), keyof typeof Base> | 'primary' | 'accent' | 'error' | 'bright' | 'dimmed';
