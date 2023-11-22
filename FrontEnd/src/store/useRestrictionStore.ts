import { Restriction } from '@/model/restriction';
import axiosIns from '@/plugins/axios';
import { defineStore } from 'pinia';

export const useRestrictionStore = defineStore('counter', {
    state: (): { data: Restriction[] | any, restriction: Restriction | null} => ({
        data: [],
        restriction: null
    }),
    actions: {
        async getRestrictions() {
            await axiosIns.get<Restriction[]>('Restrictions').then((response) => {
                this.data = response;
            });
        },

        async getRestrictionByBuildingId(id: number) {
            await axiosIns.get<Restriction>('Restrictions/' + id).then((response) => {
                this.restriction = response.data;
            });
        },

        
    },
});
