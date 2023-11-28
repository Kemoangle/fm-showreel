import { Category } from '@/model/category';
import axiosIns from '@/plugins/axios';
import { defineStore } from 'pinia';

export const useCategoryStore = defineStore('category', {
    state: (): { data: any, video: Category | null} => ({
        data: [],
        video: null
    }),
    actions: {
        async getAllCategory() {
            await axiosIns.get<Category[]>('Category').then((response) => {
                this.data = response;
            });
        },
        async getCategoryByVideo(id: number) {
            await axiosIns.get<Category[]>('Category/GetCategoryByVideo/' + id).then((response) => {
                this.data = response;
            });
        },
        async updateVideoCategory(videoId: number | undefined, categories: Category[] | undefined) {
            await axiosIns.patch('Category/' + videoId, categories).then((response) => {
            });
        },
    },
});

