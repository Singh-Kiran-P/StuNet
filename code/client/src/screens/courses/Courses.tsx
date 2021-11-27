// @Kiran
import React, { axios, Screen, useState } from '@/.';
import { View, Button, SearchBar, Text, ScrollView } from '@/components';
import List_ from '@/components/base/List';
import { Course } from '../course/Course';
import { showMessage, hideMessage } from "react-native-flash-message";
import FlashMessage from "react-native-flash-message";

export interface IState {
    courses: Course[];
}

export default Screen('Courses', ({ params, nav }) => {
    const [query, setQuery] = useState('TEST');
    const [courses, setCourses] = useState<IState["courses"]>([]);
    // TODO: error door dat de question leeg is
    const search = async () => {
        return axios.get<IState["courses"]>('/Course/search', { params: {name : query}} )
            .then(res => {
                console.log(res.status);

                if (res.status == 200) {
                    console.log(res.data);
                    setCourses(res.data)
                }
                else {
                    showMessage({
                        message: "Could not load any courses with this name",
                        type: "info",
                        position: "bottom",
                        floating: true,
                        icon: 'info'
                    });
                }


            })
            .catch(err => {
                showMessage({
                    message: "Problems reaching course data end-point",
                    type: "danger",
                    position: "bottom",
                    floating: true,
                    icon: 'danger'
                });
            })
    }

    return (
        <View style={{ flex: 1 }}>
            <SearchBar placeholder="sdf" onChangeText={q => setQuery(q)} />
            <Button onPress={search}>Search</Button>
            <ScrollView>
                <List_ courses={courses} />
            </ScrollView>
            <FlashMessage />
        </View>
    );
});
