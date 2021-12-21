import React, { extend, BaseTopic, Theme } from '@/.';
import { View, Chip, IconButton, Touchable } from '@/components/base';

type Props = {
    topics: BaseTopic[];
    actives: number[];
    setActives: (actives: number[]) => void;
    start?: React.ReactElement;
}

export default extend<typeof View, Props>(View, ({ topics = [], actives, setActives, start, ...props }) => {
    if (!topics.length) return null;

    return (
        <View type='header' {...props}>
            {start || null}
            {topics.map(({ name, id }, i) => (
                <Touchable margin={i < topics.length - 1 ? 'bottom,right' : 'bottom'} radius='round' borderless key={i} onPress={() => {
                    let i = actives.findIndex(i => i === id);
                    if (i < 0) return setActives(actives.concat(id));
                    setActives(actives.slice(0, i).concat(actives.slice(i + 1)));
                }} children={<Chip children={name} size='normal' active={actives.includes(id)}/>}/>
            ))}
            <IconButton icon='close' size={Theme.bigger} disabled={!actives.length} onPress={() => setActives([])}/>
        </View>
    )
})
