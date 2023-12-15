import { Category } from '@/model/category';
import { Video } from '@/model/video';
import axiosIns from '@/plugins/axios';
import { defineStore } from 'pinia';

export const useCategoryStore = defineStore('category', {
    state: (): { data: any, pageCategory: any} => ({
        data: [],
        pageCategory: []
    }),
    actions: {
        async getAllCategory() {
            await axiosIns.get<Category[]>('Category/GetParent').then((response) => {
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

        async getPageCategory(keySearch: string, page: number, pageSize: number) {
            await axiosIns.get<Video[]>('Category/GetPageCategory', {
                params: {
                    keySearch: keySearch,
                    page: page,
                    pageSize: pageSize
                }
            }).then((response) => {
                this.pageCategory = response;
            });
        },

        async addCategory(category: Category): Promise<Category> {
            return await axiosIns.post('Category', category);
        },
        
        async updateCategory(category: Category): Promise<Category>  {
            return await axiosIns.patch('Category/UpdateCategory/' + category.id, category);
        },

        async getSubCategory(parentId: number) {
            return await axiosIns.get<Category[]>('Category/GetCategoryByParent/' + parentId);
        },

        async updateSubCategory(categories: Category[] | undefined, id: number) {
            return await axiosIns.patch('Category/UpdateSubCategory/' + id, categories);
        },
    },
});

