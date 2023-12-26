import { Building } from '@/model/building';
import { Category } from '@/model/category';
import axiosIns from '@/plugins/axios';
import { defineStore } from 'pinia';

export const useRuleStore = defineStore('rules', {
    state: (): { dataDoNotPlay: Building[]; dataNoBackToBack: Category[] } => ({
        dataDoNotPlay: [],
        dataNoBackToBack: [],
    }),
    actions: {
        async UpdateDoNotPlay(buildings: Building[] | undefined, videoId: number | undefined) {
            return (await axiosIns.patch('Rules/UpdateDoNotPlay/' + videoId, buildings)).data;
        },
        async UpdateNoBackToBack(categories: Category[] | undefined, videoId: number | undefined) {
            return (await axiosIns.patch('Rules/UpdateNoBackToBack/' + videoId, categories)).data;
        },
    },
});
