export interface IVideos {
    key: string;
    name: string;
    durations?: number;
    loop: number;
    category?: string;
    restriction: string;
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
    category?: string;
    remarks: string;
    order: number;
}

export interface IListPlaylist {
    id: number;
    buildingName: string;
    playlist: IPlaylist[];
}
