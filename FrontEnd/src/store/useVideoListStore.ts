import { VideoList } from '@/model/videoList';
import axiosIns from '@/plugins/axios';
import { defineStore } from 'pinia';

export const useVideoListStore = defineStore('videoList', {
    state: (): { data: any} => ({
        data: []
    }),
    actions: {
        async getAllVideoList() {
            await axiosIns.get<VideoList[]>('VideoList/getAll').then((response) => {
            });
        },
        async getPageVideoList(keySearch: string, page: number, pageSize: number) {
            await axiosIns.get<VideoList[]>('VideoList', {
                params: {
                    keySearch: keySearch,
                    page: page,
                    pageSize: pageSize
                }
            }).then((response) => {
                this.data = response;
            });
        },
    },
});

