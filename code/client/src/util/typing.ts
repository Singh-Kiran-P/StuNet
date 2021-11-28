import { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { RouteProp } from '@react-navigation/native';

export type Children = { children: React.ReactNode };

export type Route = { navigation: NativeStackNavigationProp<any>, route: RouteProp<any> };

export type Props<Component extends (keyof JSX.IntrinsicElements | React.JSXElementConstructor<any>)> = React.ComponentProps<Component>;
