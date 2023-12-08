import { VideoType } from '@/model/videoType';
import axiosIns from '@/plugins/axios';
import { defineStore } from 'pinia';

export const useVideoTypeStore = defineStore('videoType', {
    state: (): { data: VideoType[] | any, videoTypes: VideoType[] | any} => ({
        data: [],
        videoTypes: []
    }),
    actions: {
        async getAllVideoType() {
            await axiosIns.get<VideoType[]>('VideoType').then((response) => {
                this.videoTypes = response;
            });
        },

        async getPageVideoType(keySearch: string, page: number, pageSize: number) {
            await axiosIns.get<VideoType[]>('VideoType/GetPageVideotype', {
                params: {
                    keySearch: keySearch,
                    page: page,
                    pageSize: pageSize
                }
            }).then((response) => {
                this.data = response;
            });
        },

        async addVideoType(videoType: VideoType) {
            return await axiosIns.post('VideoType', videoType);
        },
        
        async updateVideoType(videoType: VideoType) {
            return await axiosIns.patch('VideoType/' + videoType.id, videoType);
        },
    },
});
