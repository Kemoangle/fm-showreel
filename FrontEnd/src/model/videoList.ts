import { VideoVideolist } from "./videoVideolist"

export interface VideoList{
    id?: number
    title?: string
    remark?: string
    createdTime?: Date
    lastUpdatedTime?: Date
    videoVideoList?: VideoVideolist[]
}
