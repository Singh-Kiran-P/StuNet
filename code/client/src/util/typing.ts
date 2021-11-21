
export type Children = { children: React.ReactNode };


export type Props<Component extends (keyof JSX.IntrinsicElements | React.JSXElementConstructor<any>)> = React.ComponentProps<Component>;
