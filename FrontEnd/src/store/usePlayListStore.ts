import { IPostPlaylistStore } from '@/model/playlist';
import axiosIns from '@/plugins/axios';
import { defineStore } from 'pinia';

interface IPagination {
    currentPage: number;
    pageSize: number;
    totalItems: number;
    totalPages: number;
}
interface IStore {
    data: IPostPlaylistStore[] | any;
    playlistBuilding: IPostPlaylistStore[] | any;
    isLoading: boolean;
    infoPage: IPagination;
}

interface IGetPlaylist {
    currentPage: number;
    pageSize: number;
    playlist: IPostPlaylistStore[];
    totalItems: number;
    totalPages: number;
}

export const usePlaylistStore = defineStore('playlist', {
    state: (): IStore => ({
        data: [],
        playlistBuilding: [],
        isLoading: false,
        infoPage: {
            currentPage: 1,
            pageSize: 10,
            totalItems: 10,
            totalPages: 1,
        },
    }),
    actions: {
        async getPlaylists(keySearch: string = '', page: number = 1, pageSize: number = 10) {
            this.isLoading = true;
            return await axiosIns
                .get<IGetPlaylist>('PlayList', {
                    params: {
                        keySearch,
                        page,
                        pageSize,
                    },
                })
                .then((res) => {
                    this.isLoading = false;
                    const data = res.data;

                    this.data = data.playlist;
                    this.infoPage = { ...data } as IPagination;
                    return this.data;
                })
                .catch((err) => {
                    this.isLoading = true;
                    return err;
                });
        },

        async getPlaylistChildren(
            id: number,
            keySearch: string,
            page: number,
            pageSize: number
        ) {
            this.isLoading = true;
            await axiosIns
                .get<IGetPlaylist>('PlayList/GetPlayListByParent/' + id, {
                    params: {
                        keySearch,
                        page,
                        pageSize,
                    },
                })
                .then((res) => {
                    this.isLoading = false;

                    this.playlistBuilding = res.data;
                })
                .catch((err) => {
                    this.isLoading = true;
                });
        },

        async addNewPlaylist(data: IPostPlaylistStore[]) {
            return await axiosIns.post('PlayList', data);
        },

        async updatePlaylist(data: IPostPlaylistStore) {
            return await axiosIns.patch('PlayList/' + data.id, data).then((res) => {
                const index = this.data.findIndex((x: IPostPlaylistStore) => x.id == res.data?.id);
                if (index >= 0) {
                    this.data[index] = res.data;
                }
                return res.data;
            });
        },

        async removePlaylist(id: number) {
            return await axiosIns.delete('PlayList/' + id);
        },
    },
});
