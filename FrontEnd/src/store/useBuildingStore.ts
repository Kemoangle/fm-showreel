import { Building } from '@/model/building';
import axiosIns from '@/plugins/axios';
import _ from 'lodash';
import { defineStore } from 'pinia';

export const useBuildingStore = defineStore('building', {
    state: (): { data: any; building: Building | null; allBuilding: Building[] } => ({
        data: [],
        building: null,
        allBuilding: [],
    }),
    // could also be defined as
    // state: () => ({ count: 0 })
    actions: {
        async getPageBuilding(keySearch: string, page: number, pageSize: number) {
            await axiosIns
                .get<Building[]>('Building', {
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
            return await axiosIns.post('Building', building);
        },

        async deleteBuilding(id: number) {
            return await axiosIns.delete('Building/' + id);
        },

        async updateBuilding(building: Building) {
            return await axiosIns.patch('Building/' + building.id, building);
        },

        async getAllBuildingStore() {
            if (!_.isEmpty(this.allBuilding)) {
                return this.allBuilding;
            } else {
                return (await axiosIns
                    .get<Building[]>('Building/getAll', {
                        params: {
                            isGetLandlord: true,
                        },
                    })
                    .then((response: any) => {
                        this.allBuilding = response;
                        return response;
                    })) as Building[];
            }
        },
    },
});
