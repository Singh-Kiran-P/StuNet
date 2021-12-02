import { screens, tabs } from '@/nav/types';
import { Theme } from '@/css';

export const s = screens({

    Question: {
        title: 'Question ({id})',
        args: {} as {
            id: number;
        }
    },
    CreateAnswer: {
        title: 'Answer ({questionId})',
        args: {} as {
            date: string;
            question: string;
            questionId: number;
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
            courseId: number;
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
        colors: 'home'
    },

    TabCourses: {
        screen: 'Courses',
        title: 'Courses',
        icon: 'book',
        colors: 'courses'
    },

    TabNotifications: {
        screen: 'Notifications',
        title: 'Notifications',
        icon: 'bell',
        colors: 'notifications'
    },

    TabProfile: {
        screen: 'Profile',
        title: 'Profile',
        icon: 'face',
        colors: 'profile'
    }

})
