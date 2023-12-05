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

