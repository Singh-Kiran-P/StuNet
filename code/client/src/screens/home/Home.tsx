import React, { Screen } from '@/.';
import { View, Button } from '@/components';
import notifee, { AndroidImportance } from '@notifee/react-native';

export default Screen('Home', ({ nav }) => {

    async function onDisplayNotification() {
        // Create a channel
        const channelId = await notifee.createChannel({
          id: 'default',
          name: 'Default Channel',
        });
    
        try {
            // https://notifee.app/react-native/docs/android/appearance
            // Display a notification
            await notifee.displayNotification({
                title: 'Notification Title',
                body: 'Main body content of the notification',
                android: {
                  importance: AndroidImportance.HIGH,
                    channelId,
                    smallIcon: 'ic_launcher', // optional, defaults to 'ic_launcher'.
                },
            });
        } catch (e) {
            console.log(e)
        }
      }
    

    return (
        <View>
            <Button children='Course' onPress={() => nav.push('Course', { id: 1 })}/>
            <Button margin children='Question' onPress={() => nav.push('Question', { id: 1 })}/>
            <Button margin children='Notification' onPress={() => onDisplayNotification()}/>
        </View>
    )
})
