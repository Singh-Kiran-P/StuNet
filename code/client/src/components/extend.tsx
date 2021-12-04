import React, { Style, Theme } from '@/.';
import { GetProps } from '@/util';

type Props = {
    hidden?: any;
    alignRight?: any;
    margin?: boolean | 'bottom' | 'top' | 'left' | 'right' | 'vertical' | 'horizontal' | 'all';
}

const property = (margin: Props['margin']) => {
    if (!margin) return '';
    if (typeof margin !== 'string') margin = 'top';
    switch (margin) {
        case 'bottom': return'marginBottom';
        case 'top': return 'marginTop';
        case 'left': return 'marginLeft';
        case 'right': return 'marginRight';
        case 'vertical': return 'marginVertical';
        case 'horizontal': return 'marginHorizontal';
        case 'all': return 'margin';
    }
}

export default <T extends React.JSXElementConstructor<any>, U extends {} = {}>(c: T, e: (p: GetProps<T> & Props & Omit<U, keyof Props>) => JSX.Element | null) => {

    const Extend = ({ hidden, margin, alignRight, ...props }: Partial<Omit<GetProps<T>, keyof (Props & U)> & Props & U>) =>  {
        if (hidden) return null;

        const s = Style.create({    
            margin: !margin ? {} : {
                [property(margin)]: Theme.margin
            },

            right: !alignRight ? {} : {
                marginLeft: 'auto'
            }
        })

        return e({ ...props, style: [s.margin, s.right, (props as any).style] } as any);
    }

    return Object.assign(Extend, c as Omit<T, keyof (GetProps<T> & Props & U)>);
}
