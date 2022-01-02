import React, { Style, Theme } from '@/.';
import { GetProps } from '@/util';

type Side = 'bottom' | 'top' | 'left' | 'right' | 'vertical' | 'horizontal' | 'all';
type Factor = '0.2' | '0.5' | '1.5' | '2';
type Value = Side | `${Side}-${Factor}`;
type Values = Value | `${Value},${Value}`;

export type Props = {
    hidden?: any;
    
    flex?: boolean | number;
    grow?: boolean | number;
    shrink?: boolean | number;

    margin?: boolean | Values;
    padding?: boolean | Values;
    radius?: boolean | 'round';
    align?: 'right' | 'bottom';
    pad?: boolean | 'top' | 'bottom';
    inner?: boolean;
}

const values = (s: string, values: undefined | boolean | Values, d: Side) => {
    return typeof values === 'string' ? values.split(',').map(v => value(s, v as Value, d)) : [value(s, values, d)];
}

const value = (s: string, value: undefined | boolean | Value, d: Side) => {
    let v = (typeof value === 'string' ? value.split('-') : [d]) as [Side, string];
    return [side(s, v[0], d), value ? parseFloat(v[1]) || 1 : 0] as [Side, number];
}

const reverse = (side: Side): Side => {
    switch (side) {
        case 'bottom': return 'top';
        case 'top': return 'bottom';
        case 'left': return 'right';
        case 'right': return 'left';
        case 'vertical': return 'vertical';
        case 'horizontal': return 'horizontal';
        case 'all': return 'all';
    }
}

const side = (s: string, margin: undefined | boolean | Side, d: Side) => {
    if (!margin) return '';
    if (typeof margin !== 'string') margin = d;
    switch (margin) {
        case 'bottom': return s + 'Bottom';
        case 'top': return s + 'Top';
        case 'left': return s + 'Left';
        case 'right': return s + 'Right';
        case 'vertical': return s + 'Vertical';
        case 'horizontal': return s + 'Horizontal';
        case 'all': return s;
    }
}

export default <T extends React.JSXElementConstructor<any>, U extends {} = {}>(c: T, e: (p: GetProps<T> & Props & Omit<U, keyof Props>) => JSX.Element | null) => {

    const Extend = ({ hidden, flex, grow, shrink, margin, padding, radius, align, pad, inner, ...props }: Partial<Omit<GetProps<T>, keyof (Props & U)> & Props & U>) =>  {
        if (hidden) return null;

        let margins = values('margin', margin, 'top');
        let paddings = values('padding', padding, 'all');

        const s = Style.create({
            radius: radius === undefined ? {} : {
                borderRadius: !radius ? 0 : radius === true ? Theme.radius : Theme.massive
            },

            flex: flex === undefined ? {} : {
                flex: +flex
            },

            flexGrow: grow === undefined ? {} : {
                flexGrow: !grow ? 0 : grow === true ? 1 : grow
            },

            flexShrink: shrink === undefined ? {} : {
                flexShrink: !shrink ? 0 : shrink === true ? 1 : shrink
            },

            margin: margin === undefined ? {} : Object.assign({}, ...margins.map(margin => ({
                [margin[0]]: Theme.margin * margin[1]
            }))),

            right: align === undefined ? {} : {
                [side('margin', reverse(align), 'all')]: 'auto'
            },

            pad: pad === undefined ? {} : {
                marginLeft: Theme.padding,
                marginRight: Theme.padding,
                [side('margin', pad, 'horizontal')]: Theme.padding
            }
        })

        const p = Style.create({
            padding: padding === undefined ? {} : Object.assign({}, ...paddings.map(padding => ({
                [padding[0]]: Theme.padding * padding[1]
            })))
        })

        if (inner) return e({ ...props,
            style: [...Object.values(s), (props as any).style],
            contentContainerStyle: [p.padding, (props as any).contentContainerStyle]
        } as any);
    
        return e({ ...props,
            style: [...Object.values(s), p.padding, (props as any).style]
        } as any);
    }

    return Object.assign(Extend, c as Omit<T, keyof (GetProps<T> & Props & U)>);
}
