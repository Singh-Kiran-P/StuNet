import React, { Style, Theme } from '@/.';
import { GetProps } from '@/util';

type Props = {
    hidden?: any;
    flex?: boolean | number;
    align?: 'right' | 'bottom';
    margin?: boolean | 'bottom' | 'top' | 'left' | 'right' | 'vertical' | 'horizontal' | 'all';
}

const marginProperty = (margin: Props['margin']) => {
    if (!margin) return '';
    if (typeof margin !== 'string') margin = 'top';
    switch (margin) {
        case 'bottom': return 'marginBottom';
        case 'top': return 'marginTop';
        case 'left': return 'marginLeft';
        case 'right': return 'marginRight';
        case 'vertical': return 'marginVertical';
        case 'horizontal': return 'marginHorizontal';
        case 'all': return 'margin';
    }
}

const alignProperty = (align: Props['align']) => {
    if (!align) return '';
    switch (align) {
        case 'right': return 'marginLeft';
        case 'bottom': return 'marginTop';
    }
}

export default <T extends React.JSXElementConstructor<any>, U extends {} = {}>(c: T, e: (p: GetProps<T> & Props & Omit<U, keyof Props>) => JSX.Element | null) => {

    const Extend = ({ hidden, flex, margin, align, ...props }: Partial<Omit<GetProps<T>, keyof (Props & U)> & Props & U>) =>  {
        if (hidden) return null;

        const s = Style.create({
            flex: flex === undefined ? {} : {
                'flex': +flex
            },

            margin: !margin ? {} : {
                [marginProperty(margin)]: Theme.margin
            },

            right: !align ? {} : {
                [alignProperty(align)]: 'auto'
            }
        })

        return e({ ...props, style: [s.flex, s.margin, s.right, (props as any).style] } as any);
    }

    return Object.assign(Extend, c as Omit<T, keyof (GetProps<T> & Props & U)>);
}
