import { Restriction } from './restriction';
import { Video } from './video';

export interface Building {
    id: number;
    buildingName?: string;
    address?: string;
    district?: string;
    remark?: string;
    createTime?: Date;
    lastUpdateTime?: Date;
    zone?: string;
    postalCode?: number;
    restrictions?: Restriction[];
}

export interface IBuildingLandlord {
    buildingId: number;
    videos: Video[] | any;
}

export interface IBuildingLandlordGroup {
    buildingId: number[];
    videos: Video[] | any;
}

export interface IBuildingRestriction {
    buildingId: number;
    restriction: Restriction[];
}

export interface IBuildingRestrictionGroup {
    buildingId: number[];
    restriction: Restriction[];
}

export interface IDetailBuilding {
    id: number;
    lanlordAds: IBuildingLandlord[];
    restriction: Restriction[];
}

export interface IDetailBuildingGroup {
    id: number[];
    lanlordAds: IBuildingLandlord[];
    restriction: Restriction[];
}
