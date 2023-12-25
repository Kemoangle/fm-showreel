type statusPlaylist = 'pending' | 'active' | 'expired';

export interface IPostPlaylistStore {
    id: number;
    status: statusPlaylist;
    startDate?: string;
    endDate?: string;
    title: string;
    jsonPlaylist: string;
    creator: string;
    parentId: number;
}
