import { LandlordAds } from '@/model/landlordAds';
import axiosIns from '@/plugins/axios';
import { defineStore } from 'pinia';

export const useLanlordAdsStore = defineStore('landlord', {
    state: (): { data: LandlordAds[]; landlordAds: LandlordAds | null } => ({
        data: [],
        landlordAds: null,
    }),
    actions: {
        async getAllLandlordAds(id: number) {
            await axiosIns.get<LandlordAds[]>('LandlordAds/' + id).then((response: any) => {
                this.data = response.data;
            });
        },

        async addLandlordAds(landlordAds: LandlordAds): Promise<LandlordAds> {
            return (await axiosIns.post('LandlordAds', landlordAds)).data;
        },

        async deleteLandlordAds(id: number) {
            return (await axiosIns.delete('LandlordAds/' + id)).data;
        },

        async updateLandlordAds(landlordAds: LandlordAds): Promise<LandlordAds> {
            return (await axiosIns.patch('LandlordAds/' + landlordAds.id, landlordAds)).data;
        },
    },
});
