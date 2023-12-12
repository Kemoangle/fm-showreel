import { Building } from '@/model/building';
import axiosIns from '@/plugins/axios';
import _ from 'lodash';
import { Video } from '@/model/video';
import { LandlordAds } from '@/model/landlordAds';
import { IVideos } from '@/model/generatorPlaylist';
import { defineStore } from 'pinia';

interface IState {
    data: any;
    building: Building | null;
    allBuilding: Building[];
    listIdBuildingActive: number[];
    isLoadingLandlordAds: boolean;
}

export interface IBuildingLandlord {
    buildingId: number;
    videos: Video[] | any;
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
                    .get<Building[]>('Building/getAll')
                    .then((response: Building[] | any) => {
                        this.allBuilding = response;
                        return response;
                    })) as Building[];
            }
        },

        async getLandlordBuilding(id: number) {
            return await axiosIns.get<LandlordAds[]>('LandlordAds/building/' + id);
        },

        async setListBuildingActive(
            ids: number[],
            callBack: (LandlordAds: IBuildingLandlord[]) => void
        ) {
            this.listIdBuildingActive = ids;

            let arrLandLordAds: IBuildingLandlord[] = [];

            const promise = new Promise(async (resolve, reject) => {
                ids.forEach(async (id) => {
                    const landlordAds = await this.getLandlordBuilding(id);
                    arrLandLordAds.push({
                        buildingId: id,
                        videos: convertVideoLandlordToVideo(landlordAds),
                    });
                    if (arrLandLordAds.length == ids.length) {
                        resolve('');
                    }
                });
            });

            promise.then((data) => {
                callBack(arrLandLordAds);
            });
        },
    },
});
