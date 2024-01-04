import moment from 'moment';
import _ from 'lodash';

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

export const mergeBuildings = (data: any) => {
    const buildings = _.values(
        _.groupBy(ConvertObjRemoveId(data), (item) => JSON.stringify(_.omit(item, 'id')))
    ).map((group) => {
        const ids = _.map(group, 'id');
        return _.assign(_.omit(group[0], 'id'), { id: ids });
    });

    return buildings;
};

function handleRemoveId(obj: any, index: number = 0) {
    if (obj && typeof obj === 'object') {
        for (let key in obj) {
            if (key === 'id' && index != 0) {
                obj[key] = null;
            } else {
                handleRemoveId(obj[key], index + 1);
            }
        }
    }
}

export const ConvertObjRemoveId = (arr: any) => {
    return arr.map((item: any, index: number) => {
        const newItem = { ...item };
        handleRemoveId(newItem);
        return newItem;
    });
};
