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
