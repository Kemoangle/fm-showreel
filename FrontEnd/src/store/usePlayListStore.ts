import axiosIns from '@/plugins/axios';
import { defineStore } from 'pinia';
import { IPostPlaylistStore } from '@/model/playlist';
import _ from 'lodash';

interface IStore {
    data: IPostPlaylistStore[] | any;
    isLoading: boolean;
}

export const usePlaylistStore = defineStore('playlist', {
    state: (): IStore => ({
        data: [],
        isLoading: false,
    }),
    actions: {
        async getPlaylists(keySearch: string, currentPage: number, pageSize: number) {
            this.isLoading = true;
            await axiosIns
                .get<IPostPlaylistStore[]>('PlayList', {
                    params: {
                        keySearch,
                        currentPage,
                        pageSize,
                    },
                })
                .then((res) => {
                    this.isLoading = false;

                    this.data = res.playlist;
                })
                .catch((err) => {
                    this.isLoading = true;
                });
        },

        async addNewPlaylist(data: IPostPlaylistStore) {
            return await axiosIns.post('PlayList', data);
        },

        async updatePlaylist(data: IPostPlaylistStore) {
            return await axiosIns.patch('PlayList/' + data.id, data).then((res) => {
                const index = this.data.findIndex((x: IPostPlaylistStore) => x.id == res?.id);
                if (index >= 0) {
                    this.data[index] = res;
                }
                return res;
            });
        },

        async removePlaylist(id: number) {
            return await axiosIns.delete('PlayList/' + id);
        },
    },
});
