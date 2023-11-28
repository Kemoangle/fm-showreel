export interface IVideos {
    key: number;
    name: string;
    durations: number;
    loop: number;
    category: string;
    restriction: string;
}

export interface IListInfoCompany {
    companyName: string;
    landlordAds: never[];
    listVideos: IVideos[];
}

export interface IPlaylist {
    title: string;
    duration: number;
    keyNo: string;
    category: string;
    remasks: string;
}

export interface IListPlaylist {
    buildingName: string;
    playlist: IPlaylist[];
}
