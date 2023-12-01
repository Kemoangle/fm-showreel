import { VideoList } from '@/model/videoList';
import { VideoVideolist } from '@/model/videoVideolist';
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
        
        
        async addVideoList(videoList: VideoList): Promise<VideoList> {
            return await axiosIns.post('VideoList/' , videoList);
        },

        async addVideoVideoList(videoVideolist: VideoVideolist[] | undefined, id: number | undefined) {
            await axiosIns.patch('VideoList/' + id , videoVideolist);
        },

        async getVideoByListId(id: number) {
            return await axiosIns.get<VideoVideolist[]>('VideoList/' + id);
        },

        async deleteList(id: number) {
            await axiosIns.delete('VideoList/' + id);
        },
    },                             
});

