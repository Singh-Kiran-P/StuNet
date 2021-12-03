import { screens, tabs } from '@/nav/types';
import { Answer } from '@/util/types';

export const s = screens({

    Question: {
        args: {} as {
            id: number;
        }
    },
    CreateAnswer: {
        screenTitle: 'Answer ({questionId})',
        args: {} as {
            date: string;
            question: string;
            questionId: number;
        }
    },
    Answer: {
        screenTitle: '{course}',
        args: {} as Answer & {
            course: string
        }
    },

    Course: {
        screenTitle: 'Course ({id})',
        args: {} as {
            id: number
        }
    },
    AskQuestion: {
        screenTitle: 'Ask Question ({courseId})',
        args: {} as {
            courseId: number;
        }
    },
    EditCourse: {
        title: 'Edit course ({id})',
        args: {} as {
            id: number
        }
    },
    EditTopics: {
        title: 'Edit topics of course ({id})',
        args: {} as {
            courseId: number
        }
    },



    Home: {
        screenTitle: 'Home'
    },



    Courses: {
        screenTitle: 'Search For Courses'
    },
    CreateCourse: {
        screenTitle: 'Create Your Course'
    },


    Notifications: {
        screenTitle: 'Your Notifications'
    },



    Profile: {
        screenTitle: 'Your Profile',
    },
    EditProfile: {
        screenTitle: 'Edit Your Profile'
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
