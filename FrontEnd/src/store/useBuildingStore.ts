import { Building } from '@/model/building';
import axiosIns from '@/plugins/axios';
import { defineStore } from 'pinia';

export const useBuildingStore = defineStore('building', {
    state: (): { data: any, building: Building | null} => ({
        data: [],
        building: null
    }),
    // could also be defined as
    // state: () => ({ count: 0 })
    actions: {
        async getPageBuilding(keySearch: string, page: number, pageSize: number) {
            
            await axiosIns.get<Building[]>('Building', {
                params: {
                    keySearch: keySearch,
                    page: page,
                    pageSize: pageSize
                }
            }).then((response) => {
                this.data = response;
            });
        },
        

        async getAllBuilding() {
            await axiosIns.get<Building[]>('Building/getAll').then((response) => {
                this.data.buildings = response;
            });
        },

         getBuildingById(id: number) {
            var item = this.data.buildings.find((d: Building) => d.id == id);
            this.building = item;
            return item;
        },

        async addBuilding(building: Building) {
            await axiosIns.post('Building', building).then((response) => {
            });
        },
        async deleteBuilding(id: number) {
            await axiosIns.delete('Building/' + id).then((response) => {
            });
        },

        async updateBuilding(building: Building) {
            await axiosIns.patch('Building/' + building.id, building).then((response) => {
            });
        },
    },
});

