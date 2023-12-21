import moment from 'moment';

export const getTimestamp = () => {
    const date = moment();
    return (
        date.year() +
        '' +
        +date.month() +
        1 +
        '' +
        +date.date() +
        1 +
        '' +
        date.hour() +
        '' +
        date.minute() +
        '' +
        date.second()
    );
};
