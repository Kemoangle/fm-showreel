import { Video } from "./video"

export interface VideoVideolist{
    id?: number
    videoId?: number
    videoName?: string
    videoListId?: number
    videoListName?: string
    loopNum?: number
    video?: Video
}
