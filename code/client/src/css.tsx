import { StyleSheet } from 'react-native';
import { DefaultTheme } from 'react-native-paper';

export const Theme = {
    ...DefaultTheme,
    large: 24,
    medium: 16,
    small: 12,

    padding: 10
}

export const text = StyleSheet.create({
    header: {
        fontSize: Theme.large,
        fontWeight: 'bold'
    }
});
