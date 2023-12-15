import { Building } from './building';
import { Category } from './category';
import { Video } from './video';
import { VideoVideolist } from './videoVideolist';

export interface VideoList {
    id?: number;
    title?: string;
    remark?: string;
    createdTime?: Date;
    lastUpdatedTime?: Date;
    videoVideoList?: VideoVideolist[];
}

export interface IVideoInList {
    video: Video;
    category: Category[];
    loopNum: number;
    noBackToBack?: Category[];
    doNotPlay?: Building[];
}
