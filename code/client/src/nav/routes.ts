import { screens, tabs } from '@/nav/types';
import { Course, Question } from '@/util/types';

export const s = screens({

    Home: {
        screenTitle: 'StuNet',
        padding: false,
        scroll: false,
        tabs: true
    },



    Courses: {
        screenTitle: 'Search For Courses',
        padding: false,
        scroll: false,
        search: '',
        tabs: true
    },
    CreateCourse: {
        screenTitle: 'Create Your Course',
        tabs: true
    },



    Course: {
        screenTitle: '{name}',
        subscribe: null,
        padding: false,
        scroll: false,
        name: '',
        args: {} as {
            id: number;
        }
    },
    Channel: {
        screenTitle: '{name} in {course}',
        padding: false,
        scroll: false,
        course: '',
        name: '',
        args: {} as {
            id: number
        }
    },
    Questions: {
        screenTitle: 'Questions in {course.name}',
        padding: false,
        scroll: false,
        search: '',
        args: {} as {
            course: Course
        }
    },
    AskQuestion: {
        screenTitle: '{course.name}',
        selected: [] as number[],
        args: {} as {
            course: Course;
        }
    },
    EditCourse: {
        screenTitle: 'Edit {course.name}',
        args: {} as {
            course: Course;
        }
    },
    EditTopics: {
        screenTitle: 'Edit topics of {course.name}',
        args: {} as {
            course: Course;
        }
    },
    EditChannels: {
        screenTitle: 'Edit channels of {course.name}',
        args: {} as {
            course: Course;
        }
    },



    Question: {
        screenTitle: '{course}',
        padding: false,
        subscribe: null,
        scroll: false,
        course: '',
        args: {} as {
            id: number;
        }
    },
    GiveAnswer: {
        screenTitle: '{question.course.name}',
        args: {} as {
            question: Question
        }
    },
    Answer: {
        screenTitle: '{course}',
        course: '',
        args: {} as {
            id: number;
        }
    },



    Notifications: {
        screenTitle: 'Your Notifications',
        padding: false,
        scroll: false,
        tabs: true
    },



    Profile: {
        screenTitle: '',
        padding: false,
        scroll: false,
        logout: true,
        email: ''
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
        icon: 'account',
        colors: 'profile'
    }

})
