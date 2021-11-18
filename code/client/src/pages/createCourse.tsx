import React, { useEffect, useState } from 'react';
// import CheckboxItem from '@components/CheckboxItem';
import Page from '@components/page';
import MutuableStringList from 'components/MutableStringList';
//import { text } from '@css';

import {
    LayoutAnimation,
    Text,
    View
} from 'react-native';

import {
    ActivityIndicator,
    Button,
    List,
    TextInput,
} from 'react-native-paper';

export default function CreateCourse() {
    //const [loading, setLoading] = useState(true);
    
    const [name, setName] = useState('')
    const [number, setNumber] = useState('')
    const [description, setDescription] = useState('')
    const [staff, setStaff] = useState('')
    const [staffs, setStaffs] = useState<string[]>([]);
    //const [checks, setChecks] = useState<boolean[]>([]);

    useEffect(() => {
        //setLoading(true)
        setTimeout(() => {
            setStaffs([
                'Topic 1',
                'Topic 2',
                'Topic 3',
                'Topic 4'
            ]);
            //setChecks(topics.map(() => false));
            //setLoading(false);
        }, 1000);
    }, []);

    const submit = () => {
        console.log(name);
        console.log(number);
        console.log(staff);
        //console.log(checks.map((checked, i) => checked ? topics[i] : null).filter(x => x !== null));
    };

    return (
        // <ActivityIndicator size='large' style={{ flex: 1 }}/>
        // :
        <Page title='Create Course'>
            <MutuableStringList title="Channel"/>
            {/* <TextInput mode='outlined' label='Name' onChangeText={setName} />
            <TextInput mode='outlined' label='Number' onChangeText={setNumber} />
            <TextInput mode='outlined' label='Description' multiline numberOfLines={5} onChangeText={setDescription} />
            <TextInput mode='outlined' label='Staff e-mail:' onChangeText={setStaff} />
            <Button mode='contained' onPress={submit} disabled={ staff === ''}>Add Staff</Button>
            <MutuableStringList /> */}
            {/* <Text style={[text.header]}>Course, Subject</Text>
            <List.Accordion title='Topics' onPress={() => { LayoutAnimation.easeInEaseOut() }}>
                {topics.map((item, i) => {
                    return <CheckboxItem key={i} label={item} checked={() => checks[i]} oncheck={checked => checks[i] = checked} />
                })}
            </List.Accordion>
            */}
        </Page>)
}