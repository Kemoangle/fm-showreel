import {
    Building,
    IBuildingLandlord,
    IDetailBuilding,
    IBuildingRestriction,
} from '@/model/building';
import axiosIns from '@/plugins/axios';
import _ from 'lodash';
import { Video } from '@/model/video';
import { LandlordAds } from '@/model/landlordAds';
import { IVideos } from '@/model/generatorPlaylist';
import { defineStore } from 'pinia';
import { Restriction } from '@/model/restriction';

interface IState {
    data: any;
    building: Building | null;
    allBuilding: Building[];
    listIdBuildingActive: number[];
    isLoadingLandlordAds: boolean;
}

function convertVideoLandlordToVideo(landlordAds: LandlordAds[]) {
    const dataLandlord = _.cloneDeep(landlordAds);
    const newVideos: IVideos[] = [];
    dataLandlord.forEach((el) => {
        const loop = el.loop;
        if (loop) {
            newVideos.push({ ...el.video, loop } as IVideos);
        }
    });

    return newVideos;
}

export const useBuildingStore = defineStore('building', {
    state: (): IState => ({
        data: [],
        building: null,
        allBuilding: [],
        listIdBuildingActive: [],
        isLoadingLandlordAds: false,
    }),

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
                    this.data = response.data;
                });
        },

        async getAllBuilding() {
            await axiosIns.get<Building[]>('Building/getAll').then((response) => {
                this.data.buildings = response.data;
            });
        },

        getBuildingById(id: number) {
            var item = this.data.buildings.find((d: Building) => d.id == id);
            this.building = item;
            return item;
        },

        async addBuilding(building: Building) {
            return (await axiosIns.post('Building', building)).data;
        },

        async deleteBuilding(id: number) {
            return (await axiosIns.delete('Building/' + id)).data;
        },

        async updateBuilding(building: Building) {
            return (await axiosIns.patch('Building/' + building.id, building)).data;
        },

        async getAllBuildingStore() {
            if (!_.isEmpty(this.allBuilding)) {
                return this.allBuilding;
            } else {
                return (await axiosIns.get<Building[]>('Building/getAll').then((response) => {
                    this.allBuilding = response.data;
                    return response.data;
                })) as Building[];
            }
        },

        async getDetailBuilding(id: number) {
            const data = await axiosIns.get<IDetailBuilding>('Building/detail/' + id);
            return data.data as IDetailBuilding;
        },

        async getLandlordBuilding(id: number) {
            return (await axiosIns.get<LandlordAds[]>('LandlordAds/building/' + id)).data;
        },

        async setListBuildingActive(
            ids: number[],
            callBack: (
                LandlordAds: IBuildingLandlord[],
                restriction: IBuildingRestriction[]
            ) => void
        ) {
            this.listIdBuildingActive = ids;

            let arrLandLordAds: IBuildingLandlord[] = [];
            let restriction: IBuildingRestriction[] = [];

            const promise = new Promise(async (resolve, reject) => {
                ids.forEach(async (id) => {
                    const detail = await this.getDetailBuilding(id);
                    const landlordAds = detail.lanlordAds;
                    restriction.push({
                        buildingId: id,
                        restriction: detail.restriction,
                    });
                    arrLandLordAds.push({
                        buildingId: id,
                        videos: convertVideoLandlordToVideo(landlordAds as LandlordAds[]),
                    });
                    if (arrLandLordAds.length == ids.length) {
                        resolve('');
                    }
                });
            });

            promise.then((data) => {
                callBack(arrLandLordAds, restriction);
            });
        },
    },
});
