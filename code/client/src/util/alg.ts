const r = (s: string, o: { [key: string]: any }): string => {
    let i = s.indexOf('{');
    if (i < 0) return s;
    let j = s.indexOf('}', i);
    if (j < 0) return s;
    let name = s.slice(i + 1, j++);
    if (!(name in o)) return s.slice(0, j) + r(s.slice(j), o);
    return s.slice(0, i) + o[name] + r(s.slice(j), o);
}

export { r as replace };

export type Obj = Object & { [k: string]: any };

export const obj = (o: any) => o && typeof o === 'object' && !Array.isArray(o);
export const merge = (a: Obj, b: Obj) => Object.keys(a).reduce((c, k) => {
    if (!b.hasOwnProperty(k)) c[k] = a[k];
    else if (!obj(b[k])) c[k] = b[k];
    else c[k] = merge(a[k], b[k]);
    return c;
}, {} as Obj);

export const contains = (a: Obj = {}, b: Obj = {}): boolean => Object.entries(b).every(([k, v]) => {
    if (!(k in a) && v !== undefined) return false;
    return typeof v === 'object' ? typeof a[k] === 'object' && contains(a[k], v) : a[k] === v;
})
