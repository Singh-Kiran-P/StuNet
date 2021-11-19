import React, {Component, useState} from 'react';
import {View} from 'react-native';
import {Text} from 'react-native';
// import {accessibilityProps} from 'react-native-paper/lib/typescript/components/MaterialCommunityIcon';
import {
  // ActivityIndicator,
  Button,
  // List,
  TextInput,
} from 'react-native-paper';

type Props = {
  title: string;
  style?: object;
};

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

  return (
    <View style={props.style}>
      <TextInput
        mode="outlined"
        label={props.title}
        onChangeText={setElement}
      />
      {elements.map((item, i) => {
        return <Text key={i}>{item}</Text>;
      })}
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
