import { DefaultTheme } from 'react-native-paper';
import { StyleSheet } from 'react-native';

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
})
