import { Restriction } from '@/model/restriction';
import axiosIns from '@/plugins/axios';
import { defineStore } from 'pinia';

export const useRestrictionStore = defineStore('restriction', {
    state: (): { data: Restriction[] | any} => ({
        data: []
    }),
    actions: {
        async getRestrictionByBuildingId(id: number) {
            await axiosIns.get<Restriction[]>('Restriction/' + id).then((response) => {
                this.data = response;
            });
        },

        async addBuildingRestriction(buildingRestriction: Restriction) {
            return await axiosIns.post('Restriction', buildingRestriction);
        },

        async deleteRestriction(id: number) {
            return axiosIns.delete('Restriction/' + id).then((response) => {
                const index = this.data.findIndex((v: any) => v.id === id);
                if (index !== -1) {
                    this.data.splice(index, 1);
                }
            });
        },

        async updateBuildingRestriction(buildingRestriction: Restriction) {
            return await axiosIns.patch('Restriction/UpdateBuildingRestriction', buildingRestriction);
        },
    },
});
