
export const dateString = (date: any) => date && new Date(date).toLocaleString();

export const errorString = (err: any): string => {
    const v = (a?: any): string => {
        if (typeof a === 'object') return v(Object.values(a)[0]);
        return `${a}` || 'An unknown error has occurred';
    }
    err = err?.response?.data;
    if (typeof err !== 'object') return v(err);
    if (Array.isArray(err)) err = err[0] || {};
    if (err.description) return v(err.description);
    if (err.errors) return v(err.errors);
    return v();
}

export const show = (set: React.Dispatch<React.SetStateAction<string>>) => (err: any) => set(errorString(err));
