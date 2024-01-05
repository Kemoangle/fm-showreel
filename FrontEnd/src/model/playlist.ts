type statusPlaylist = 'pending' | 'active' | 'expired';

interface IBuildingDetail {
    id: number;
    buildingName: string;
}
export interface IPostPlaylistStore {
    id: number;
    status: statusPlaylist;
    startDate?: string;
    endDate?: string;
    title: string;
    jsonPlaylist: string;
    JsonListVideo?: string;
    creator: string;
    parentId: number;
    buildingsId?: number[];
    buildings?: IBuildingDetail[];
}
