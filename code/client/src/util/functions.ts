import 'intl';
import 'intl/locale-data/jsonp/en-US';

export const dateString = (moment: any) => {
    if (!moment) return '';
    let [datetime, now] = [new Date(moment), new Date()];
    const s = (o: Intl.DateTimeFormatOptions) => new Intl.DateTimeFormat('en-US', o).format(datetime);
    let weekday = s({ weekday: 'short' });
    let month = s({ month: 'short' });
    let day = s({ day: 'numeric' });
    let hour = s({ hour: '2-digit', hour12: false });
    let min = s({ minute: '2-digit' });
    let year = s({ year: '2-digit' });
    let current = datetime.getFullYear() === now.getFullYear();
    if (current) return `${weekday} ${month} ${day}, ${hour}:${min}`;
    return `${day} ${month} ${year}, ${hour}:${min}`;
}

export const timeDiff = (start: any, end: any) => {
    const time = (a: any) => new Date(a).getTime() || 0;
    return Math.abs(time(start) - time(end)) / 1000;
}

export const errorString = (err: any): string => {
    const v = (a?: any): string => {
        if (typeof a === 'object') return v(Object.values(a)[0]);
        return (a ? `${a}` : '') || 'An unknown error has occurred';
    }
    if (typeof err !== 'object') return v(err);
    let status = err.response?.status;
    if (status >= 500) return v();
    err = err?.response?.data;
    if (typeof err !== 'object') return v(err);
    if (Array.isArray(err)) err = err[0] || {};
    if (err.description) return v(err.description);
    if (err.errors) return (err.errors);
    if (status === 404) return v('Item not found');
    if (status === 401) return v('Unauthorized request');
    if (status === 403) return v('Unauthorized request');
    return v();
}

export const professor = (email: string) => email.endsWith('@uhasselt.be');

export const displayName = (email: string) => {
    let name = email.slice(0, (i => i < 0 ? undefined : i)(email.lastIndexOf('@')));
    return name.replace('.', ' ').split(' ').map(s => {
        return s[0].toUpperCase() + s.slice(1).toLowerCase();
    }).join(' ');
}

export const timeSort = <T extends { time: string, isAccepted?: boolean }>(items: T[]): T[] => {
    return [true, false].map(b => items.filter(item => !!item.isAccepted === b)).map(l => {
        return l.sort((a, b) => b?.time?.localeCompare(a?.time || '') || 0);
    }).flat();
}

export const show = (set: React.Dispatch<React.SetStateAction<string>>) => (err: any) => set(errorString(err));
export const display = (set: React.Dispatch<React.SetStateAction<string>>) => (err: any) => set(errorString(`${err}`));
