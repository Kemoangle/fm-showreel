// export interface IVideos {
//     key: string;
//     name: string;
//     durations?: number;
//     loop: number;
//     category?: string;
//     restriction: string;
// }

import { Category } from './category';
import { Video } from './video';

export interface IVideos extends Video {
    loop: number;
    rule?: string;
}

export interface IListInfoCompany {
    companyName: string;
    landlordAds: never[];
    listVideos: IVideos[];
}

export interface IPlaylist {
    name: string;
    durations?: number;
    key: string;
    category?: Category[];
    remarks: string;
    order: number;
}

export interface IListPlaylist {
    id: number;
    listBuilding: number[];
    buildingName: string;
    nameTimestamp: string[];
    playlist: IPlaylist[];
    isSave: boolean;
}
