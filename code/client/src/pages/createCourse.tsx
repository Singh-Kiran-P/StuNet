import React, {useEffect, useState} from 'react';
// import CheckboxItem from '@components/CheckboxItem';
import Page from '@components/page';
import axios from 'axios';
import MutuableStringList from 'components/MutableStringList';
import {
  // text,
  Theme,
} from '@css';

import {
  // LayoutAnimation,
  // Text,
  // View,
  StyleSheet,
} from 'react-native';

import {
  Button,
  // ActivityIndicator,
  // Button,
  // List,
  TextInput,
} from 'react-native-paper';
import { assertExpressionStatement } from '@babel/types';

const stylesheet = StyleSheet.create({
  stringList: {
    // marginBottom: Theme.marginBottom,
  },
});

export default function CreateCourse() {
  //const [loading, setLoading] = useState(true);

  const [name, setName] = useState('');
  const [number, setNumber] = useState('');
  const [description, setDescription] = useState('');
  const [staff, setStaff] = useState('');
  const [staffs, setStaffs] = useState<string[]>([]);
  //const [checks, setChecks] = useState<boolean[]>([]);

  useEffect(() => {
    //setLoading(true)
    setTimeout(() => {
      setStaffs(['Topic 1', 'Topic 2', 'Topic 3', 'Topic 4']);
      //setChecks(topics.map(() => false));
      //setLoading(false);
    }, 1000);
  }, []);

  const submit = () => {
    console.log("POSTING COURSE");
    axios.post("/Course", {
      name: name,
      number: number,
      //topics: topics.filter((topic, i) => checks[i]).map((topic) => topic.id)
  });
    // console.log(name);
    // console.log(number);
    // console.log(staff);
    //console.log(checks.map((checked, i) => checked ? topics[i] : null).filter(x => x !== null));
  };

  return (
    // <ActivityIndicator size="large" style={{ flex: 1 }}/>
    // :
    <Page title="Create Course">
      <TextInput mode="outlined" label="Name" onChangeText={setName} />
      <TextInput mode="outlined" label="Number" onChangeText={setNumber} />
      <MutuableStringList style={stylesheet.stringList} title="Channels" />
      <MutuableStringList style={stylesheet.stringList} title="Topics" />
      <MutuableStringList style={stylesheet.stringList} title="Assistents" />
      <TextInput
        mode="outlined"
        label="Description"
        multiline
        numberOfLines={5}
        onChangeText={setDescription}
      />
      <Button
          mode="contained"
          style={{width: '50%'}}
          onPress={submit}>
          Submit
        </Button>
    </Page>
  );
}
