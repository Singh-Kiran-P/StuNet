import { screens, tabs } from '@/nav/types';
import { Theme } from '@/css';

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
        tabs: true,
        param2: 2
    },
    test3: {
        title: 'Test 3',
        tabs: true,
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

export const t = tabs(s, {
    home: {
        screen: 'test1',
        title: 'Home',
        icon: 'home',
        colors: Theme.colors.home
    },
    courses: {
        screen: 'test2',
        title: 'Courses',
        icon: 'book',
        colors: Theme.colors.courses
    },
    notifications: {
        screen: 'test3',
        title: 'Notifications',
        icon: 'bell',
        colors: Theme.colors.notifications
    },
    profile: {
        screen: 'test4',
        title: 'Profile',
        icon: 'face',
        colors: Theme.colors.profile
    }
})
