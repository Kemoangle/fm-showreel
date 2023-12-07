import { BuildingRestriction } from '@/model/buildingRestriction';
import { VideoType } from '@/model/videoType';
import axiosIns from '@/plugins/axios';
import { defineStore } from 'pinia';

export const useRestrictionStore = defineStore('restriction', {
    state: (): { data: BuildingRestriction[] | any} => ({
        data: []
    }),
    actions: {
        async getRestrictionByBuildingId(id: number) {
            await axiosIns.get<BuildingRestriction[]>('Restriction/' + id).then((response) => {
                this.data = response;
            });
        },

        async addBuildingRestriction(buildingRestriction: BuildingRestriction): Promise<BuildingRestriction> {
            return await axiosIns.post('Restriction', buildingRestriction);
        },
        async UpdateRestrictionExcept(videoTypes: VideoType[], buildingRestrictionId: number | undefined): Promise<BuildingRestriction> {
            return await axiosIns.patch('Restriction/' + buildingRestrictionId, videoTypes);
        },
    },
});
