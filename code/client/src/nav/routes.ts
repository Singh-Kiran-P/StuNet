import { screens, tabs } from '@/nav/types';

export const s = screens({
    test1: {
        title: 'Test 1',
        param1: 1,
        args: {} as {
            name: string
        }
    },
    test2: {
        title: 'Test 2',
        param2: 2
    },
    test3: {
        title: 'Test 3',
        param3: 3
    },
    test4: {
        title: 'Test 4',
        param4: 4
    },
    test5: {
        title: 'Test 5',
        param5: 5
    }
})

export const t = tabs({
    home: {
        title: 'Home',
        icon: 'home'
    },
    courses: {
        title: 'Courses',
        icon: 'book'
    },
    notifications: {
        title: 'Notifications',
        icon: 'bell'
    },
    profile: {
        title: 'Profile',
        icon: 'face'
    }
})
