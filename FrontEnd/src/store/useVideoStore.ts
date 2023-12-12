import { Video } from '@/model/video';
import axiosIns from '@/plugins/axios';
import { defineStore } from 'pinia';

export const useVideoStore = defineStore('video', {
    state: (): { data: any, video: Video[] | any, isLoading: boolean} => ({
        data: [],
        video: [],
        isLoading: false
    }),
    actions: {
        async getAllVideos() {
            await axiosIns.get<Video[]>('Video/GetAll').then((response) => {
                this.video = response;
            });
        },
        async getPageVideo(keySearch: string, page: number, pageSize: number) {
            this.isLoading = true;
            await axiosIns.get<Video[]>('Video', {
                params: {
                    keySearch: keySearch,
                    page: page,
                    pageSize: pageSize
                }
            }).then((response) => {
                this.data = response;
                this.isLoading = false;
            });
        },

        getVideoById(id: number) {
            var item = this.data.videos.find((d: any) => d.id == id);
            return item;
        },
   
        async addVideo(video: any): Promise<Video> {
            return await axiosIns.post('Video', video);
        },
        async deleteVideo(id: number) {
            return axiosIns.delete('Video/' + id).then((response) => {
                const index = this.data.videos.findIndex((v: any) => v.id === id);
                if (index !== -1) {
                    this.data.videos.splice(index, 1);
                }
            });
        },

        async updateVideo(video: any) {
            await axiosIns.patch('Video/' + video.id, video).then((response) => {
                const index = this.data.videos.findIndex((v: any) => v.id === video.id);
                
                if (index !== -1) {
                    this.data.videos[index] = { ...this.data.videos[index], ...video };
                }
            });;

        },
        
    },
});

