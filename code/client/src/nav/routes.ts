import { screens, tabs } from '@/nav/types';
import { Course, Question } from '@/util/types';

export const s = screens({

    Home: {
        screenTitle: 'StuNet',
        scroll: false,
        tabs: true
    },



    Courses: {
        screenTitle: 'Search For Courses',
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
        scroll: false,
        name: '',
        args: {} as {
            id: number;
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



    Question: {
        screenTitle: '{course}',
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
        screenTitle: 'Your Notifications'
    },



    Profile: {
        screenTitle: 'Your Profile',
        tabs: true
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
