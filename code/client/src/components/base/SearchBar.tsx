import { Searchbar } from 'react-native-paper';
import React, { Props } from '@/.';

export default(props: Partial<Props<typeof Searchbar>>) => {
    return <Searchbar
        {...props as Props<typeof Searchbar>}

    />
}
