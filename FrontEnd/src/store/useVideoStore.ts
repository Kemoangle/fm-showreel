import { Video } from '@/model/video';
import axiosIns from '@/plugins/axios';
import { defineStore } from 'pinia';

export const useVideoStore = defineStore('video', {
    state: (): { data: any, video: Video[] | any} => ({
        data: [],
        video: []
    }),
    actions: {
        async getAllVideos() {
            await axiosIns.get<Video[]>('Video/GetAll').then((response) => {
                this.video = response;
            });
        },
        async getPageVideo(keySearch: string, page: number, pageSize: number) {
            await axiosIns.get<Video[]>('Video', {
                params: {
                    keySearch: keySearch,
                    page: page,
                    pageSize: pageSize
                }
            }).then((response) => {
                this.data = response;
            });
        },

        getVideoById(id: number) {
            var item = this.data.videos.find((d: any) => d.id == id);
            return item;
        },
   
        async addVideo(video: Video): Promise<Video> {
            try {
                const response = await axiosIns.post('Video', video);
                return response as Video;
            } catch (error) {
                console.error('Error adding video:', error);
                throw error;
            }
        },
        async deleteVideo(id: number) {
            return axiosIns.delete('Video/' + id).then((response) => {
            });
        },

        async updateVideo(video: Video) {
            await axiosIns.patch('Video/' + video.id, video).then((response) => {
            });
        },
        
    },
});

