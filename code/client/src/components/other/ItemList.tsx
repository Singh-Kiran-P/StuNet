import React, { extend } from '@/.';
import { SectionList } from '@/components/base';

type Props = {
    
}

export default extend<typeof SectionList, Props>(SectionList, ({ ...props }) => {

    return (
        <SectionList

            {...props}
        />
    )
})
