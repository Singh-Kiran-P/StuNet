import { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { ViewStyle, TextStyle, ImageStyle } from 'react-native';
import { RouteProp } from '@react-navigation/native';

export type Opt<T> = { [U in keyof T]?: Opt<T[U]> };

export type Children = { children: React.ReactNode };

export type Styling = ViewStyle | TextStyle | ImageStyle;

export type Route = { navigation: NativeStackNavigationProp<any>, route: RouteProp<any> };

export type GetProps<Component extends (keyof JSX.IntrinsicElements | React.JSXElementConstructor<any>)> = React.ComponentProps<Component>;
