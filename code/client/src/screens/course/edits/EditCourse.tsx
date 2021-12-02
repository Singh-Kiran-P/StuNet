import React, {
    Screen,
    axios,
    useState,
    // AxiosResponse, // TODO: reexport from 'axios'
    Course,
} from '@/.';
import {
    TextInput,
    Button,
    Loader,
    ScrollView,
} from '@/components';

export default Screen('EditCourse', ({ params, nav }) => {
    const [name, setName] = useState('');
    const [number, setNumber] = useState('');
    const [isUpToDate, setUpToDate] = useState<boolean>(true); /* A simple check if data has been modified, regardless of undoing*/

    function init(data: Course): void
    {
        setName(data.name);
        setNumber(data.number);
    }

    async function fetch() //: Promise<void | AxiosResponse<any, any>>
    {
        return axios.get('/Course/' + params.id).then(res => init(res.data));
    }

    function update(): void
    {
        console.log("hello update")
    }
    
    return (
        <Loader load={fetch}>
            <ScrollView>
                <TextInput
                    label='name'
                    editable
                    // TODO: maxLength={?}
                    defaultValue={name}
                    onChangeText={(value_) => { setName(value_); setUpToDate(false); console.log("changing"); }}
                    />
                <TextInput
                    label='number'
                    editable
                    // TODO: maxLength={?}
                    defaultValue={number}
                    onChangeText={(value_) => { setNumber(value_); setUpToDate(false); }}
                    />
                <Button onPress={update} disabled={isUpToDate}  children='Update'/>
            </ScrollView>
        </Loader>
    );
});
