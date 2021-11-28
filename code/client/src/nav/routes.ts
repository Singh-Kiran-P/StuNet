import { screens, tabs } from '@/nav/types';
import { Theme } from '@/css';

export const s = screens({

    Question: {
        title: 'Question ({id})',
        args: {} as {
            id: number
        }
    },



    Course: {
        title: 'Course ({id})',
        args: {} as {
            id: number
        }
    },
    AskQuestion: {
        title: 'Ask Question ({courseId})',
        args: {} as {
            courseId: number
        }
    },



    Home: {
        title: 'Home'
    },



    Courses: {
        title: 'Search For Courses'
    },
    CreateCourse: {
        title: 'Create Your Course'
    },



    Notifications: {
        title: 'Your Notifications'
    },



    Profile: {
        title: 'Your Profile',
    },
    EditProfile: {
        title: 'Edit Your Profile'
    }

})

export const t = tabs(s, {

    TabHome: {
        screen: 'Home',
        title: 'Home',
        icon: 'home',
        colors: Theme.tabs.home
    },

    TabCourses: {
        screen: 'Courses',
        title: 'Courses',
        icon: 'book',
        colors: Theme.tabs.courses
    },

    TabNotifications: {
        screen: 'Notifications',
        title: 'Notifications',
        icon: 'bell',
        colors: Theme.tabs.notifications
    },

    TabProfile: {
        screen: 'Profile',
        title: 'Profile',
        icon: 'face',
        colors: Theme.tabs.profile
    }

})
