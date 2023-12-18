import { Video } from './video';

export interface LandlordAds {
    id: number;
    loop?: number;
    videoId?: number;
    buildingId?: number;
    startDate?: string;
    endDate?: string;
    video?: Video;
}
