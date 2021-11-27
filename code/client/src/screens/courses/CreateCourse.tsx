import React, { Screen, useState, axios } from '@/.';

import {
    View,
    Button,
    TextInput
} from '@/components';

export default Screen('CreateCourse', ({ params, nav }) => {
    // TODO load

    const [name, setName] = useState('');
    const [number, setNumber] = useState('');
    const [description, setDescription] = useState('');

    const submit = () => {
        axios.post('/Course', {
            name: name,
            number: number
        })
    }

    return (
        <View>
            <TextInput label='Name' onChangeText={setName}/>
            <TextInput label='Number' onChangeText={setNumber}/>
            {/* <MutuableStringList title='Channels'/>
            <MutuableStringList title='Topics'/>
            <MutuableStringList title='Assistents'/> */}
            <TextInput label='Description' multiline onChangeText={setDescription}/>
            <Button children='Create' disabled={!name || !number || !description} onPress={submit} />

        </View>
    )
})

// TODO MutableStringList:

/*

import React, {Component, useState} from 'react';

import {
  // LayoutAnimation,
  Text,
  View,
  StyleSheet,
} from 'react-native';
import {
  Button,
  TextInput,
  // ActivityIndicator,
  // List,
} from 'react-native-paper';
import {
  Theme,
  // text,
} from '@css';

type Props = {
  title: string;
  style?: object;
};

const stylesheet = StyleSheet.create({
  title: {
    backgroundColor: Theme.colors.primary,
    color: 'white',
    padding: 10,
    borderRadius: 5,
  },
  input: {
    marginBottom: 5,
  },
  items: {
      paddingTop: 2,
  },
  item: {
    padding: 2,
    textAlign: 'center',
    backgroundColor: 'lightgray',
    borderRadius: 2,
    marginTop: 2,
    marginHorizontal: 2,
  },
});

export default function MutuableStringList(props: Props) {
  const [elements, setElements] = useState<string[]>([]);
  const [element, setElement] = useState('');

  const add = () => {
    elements.push(element);

    setElements(elements.slice());

    console.log('ADDING');
    console.log(elements);
  };

  const remove = () => {
    if (elements.length > 0) {
      elements.pop();
      setElements(elements.slice());
      console.log('Removing');
    }
  };

  const overwriteStyle: object = {
    borderWidth: 1,
    borderColor: Theme.colors.primary,
    borderRadius: 3,
    padding: 10,
    marginTop: 10,
  };
  const style: object = {...props.style!, ...overwriteStyle};

  return (
    <View style={style}>
      <Text style={stylesheet.title}>{props.title}</Text>
      <View style={stylesheet.items}>
        {elements.map((item, i) => {
            return <Text style={stylesheet.item} key={i}>{item}</Text>;
        })}
      </View>
      <TextInput style={stylesheet.input} mode="outlined" label="add" onChangeText={setElement} />
      <View style={{flexDirection: 'row'}}>
        <Button
          mode="contained"
          style={{width: '50%'}}
          onPress={add}
          disabled={element === ''}>
          Add
        </Button>
        <Button mode="contained" style={{width: '50%'}} onPress={remove}>
          Remove
        </Button>
      </View>
    </View>
  );
}

*/

