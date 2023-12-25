import moment from 'moment';

export const getTimestamp = () => {
    const date = moment();
    const years = date.year();
    const months = (date.month() + 1).toString().padStart(2, '0');
    const days = date.date().toString().padStart(2, '0');
    const hours = date.hour().toString().padStart(2, '0');
    const minutes = date.minute().toString().padStart(2, '0');
    const seconds = date.second().toString().padStart(2, '0');

    return years + '' + +months + '' + days + '' + hours + '' + minutes + '' + seconds;
};
