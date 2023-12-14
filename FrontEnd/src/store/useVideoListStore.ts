import { IVideoInList, VideoList } from '@/model/videoList';
import { VideoVideolist } from '@/model/videoVideolist';
import axiosIns from '@/plugins/axios';
import { defineStore } from 'pinia';
import _ from 'lodash';

export const useVideoListStore = defineStore('videoList', {
    state: (): { data: any; allData: any; isLoading: boolean } => ({
        data: [],
        allData: [],
        isLoading: false,
    }),
    actions: {
        async getAllVideoList() {
            this.isLoading = false;
            return await axiosIns
                .get<VideoList[]>('VideoList/getAll')
                .then((response) => {
                    if (response) {
                        this.allData = response;
                    }
                    this.isLoading = false;
                    return response;
                })
                .catch((err) => {
                    this.isLoading = true;

                    return err;
                });
        },

        async getPageVideoList(keySearch: string, page: number, pageSize: number) {
            await axiosIns
                .get<VideoList[]>('VideoList', {
                    params: {
                        keySearch: keySearch,
                        page: page,
                        pageSize: pageSize,
                    },
                })
                .then((response) => {
                    this.data = response;
                });
        },

        async addVideoList(videoList: VideoList): Promise<VideoList> {
            return await axiosIns.post('VideoList/', videoList);
        },

        async addVideoVideoList(
            videoVideolist: VideoVideolist[] | undefined,
            id: number | undefined
        ) {
            await axiosIns.patch('VideoList/' + id, videoVideolist);
        },

        async getVideoByListId(id: number) {
            return await axiosIns.get<IVideoInList[]>('VideoList/' + id);
        },

        async deleteList(id: number) {
            await axiosIns.delete('VideoList/' + id);
        },

        async getListVideoStore() {
            if (_.isEmpty(this.allData)) {
                return (await this.getAllVideoList()) as VideoList[];
            } else {
                return this.allData as VideoList[];
            }
        },
    },
});
