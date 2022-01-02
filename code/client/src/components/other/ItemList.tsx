import React, { extend, Course, Channel, Question, Answer, timeSort } from '@/.';
import { CompactCourse, CompactChannel, CompactQuestion, CompactAnswer } from '@/components/compact';
import { Text, SectionList } from '@/components/base';

type Props = {
    courses?: Course[];
    channels?: Channel[];
    questions?: Question[];
    answers?: Answer[];
    name: string;
}

const display = (name: string, section: string) => {
    if (!name) return section[0].toUpperCase() + section.slice(1);
    return `${name} ${section}`;
}

export default extend<typeof SectionList, Props>(SectionList, ({ courses, channels, questions, answers, name, ...props }) => {
    return (
        <SectionList
            inner
            padding='horizontal,bottom'
            {...props}
            sections={[
                { title: 'courses', data: courses?.reverse() || [] },
                { title: 'channels', data: channels?.reverse() || [] },
                { title: 'questions', data: timeSort(questions || []) },
                { title: 'answers', data: timeSort(answers || []) }
            ].filter(s => s.data.length)}
            renderSectionHeader={({ section }) => (
                <Text type='header' color='placeholder' margin='top-2' children={display(name, section.title)}/>
            )}
            renderItem={({ item, index, section }: any) => {
                switch (section.title) {
                    case 'courses': return <CompactCourse margin={!!index} course={item}/>
                    case 'channels': return <CompactChannel margin={!!index} channel={item}/>
                    case 'questions': return <CompactQuestion margin={!!index} question={item}/>
                    case 'answers': return <CompactAnswer margin={!!index} answer={item}/>
                    default: return null;
                }
            }}
        />
    )
})
