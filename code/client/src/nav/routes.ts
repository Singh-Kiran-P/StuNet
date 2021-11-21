import { screens, tabs } from '@/nav/types';
import { Theme } from '@/css';

export const s = screens({

    Home: {
        title: 'Home'
    },



    Courses: {
        title: 'Search For Courses'
    },



    Notifications: {
        title: 'Your Notifications'
    },



    Profile: {
        title: 'Your Profile'
    },
    EditProfile: {
        title: 'Edit Your Profile'
    }

})

export const t = tabs(s, {

    HomeTab: {
        screen: 'Home',
        title: 'Home',
        icon: 'home',
        colors: Theme.colors.home
    },

    CoursesTab: {
        screen: 'Courses',
        title: 'Courses',
        icon: 'book',
        colors: Theme.colors.courses
    },

    NotificationsTab: {
        screen: 'Notifications',
        title: 'Notifications',
        icon: 'bell',
        colors: Theme.colors.notifications
    },

    ProfileTab: {
        screen: 'Profile',
        title: 'Profile',
        icon: 'face',
        colors: Theme.colors.profile
    }

})
