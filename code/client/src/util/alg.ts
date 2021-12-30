const r = (s: string, o: { [key: string]: any }): string => {
    let i = s.indexOf('{');
    if (i < 0) return s;
    let j = s.indexOf('}', i);
    if (j < 0) return s;
    let names = s.slice(i + 1, j++).split('.');
    let [object, rec] = [o, () => r(s.slice(j), object)];
    while (names.length) {
        if (typeof o !== 'object') o = {};
        if (!(names[0] in o)) return s.slice(0, j) + rec();
        o = o[names.shift() || ''];
    }
    if (!o) return '';
    return s.slice(0, i) + o + rec();
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

export const contains = (a: Obj = {}, b: Obj = {}): boolean => Object.keys(b).every(k => {
    return typeof b[k] === 'object' ? typeof a[k] === 'object' && contains(a[k], b[k]) : a[k] === b[k];
})
