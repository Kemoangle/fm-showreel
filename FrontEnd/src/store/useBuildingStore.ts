import { Building } from '@/model/building';
import axiosIns from '@/plugins/axios';
import { defineStore } from 'pinia';

export const useBuildingStore = defineStore('counter', {
    state: (): { data: Building[] | any} => ({
        data: []
    }),
    // could also be defined as
    // state: () => ({ count: 0 })
    actions: {
        async getBuilding(keySearch: string) {
            await axiosIns.get<Building[]>('Building', {
                params: {
                    keySearch: keySearch
                }
            }).then((response) => {
                this.data = response;
            });
        },

         getBuildingById(id: number) {
            return this.data.find((d: Building) => d.id == id)
        },

        async addBuilding(building: Building) {
            await axiosIns.post('Building', building).then((response) => {
                this.data.push(response);
                this.getBuilding("");
            });
        },
        async deleteBuilding(id: number) {
            await axiosIns.delete('Building/' + id).then((response) => {
                this.getBuilding("");
            });
        },

        async updateBuilding(building: Building) {
            await axiosIns.put('Building/' + building.id, building).then((response) => {
                this.getBuilding("");
            });
        },
    },
});
