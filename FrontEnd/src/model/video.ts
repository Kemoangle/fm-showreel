import { Building } from "./building"
import { Category } from "./category"
import { VideoType } from "./videoType"

export interface Video{
    id: number
    title?: string
    filePath?: string
    duration?: number
    keyNo?: string
    remark?: string
    isActive?: boolean
    createTime?: Date
    lastUpdateTime?: Date
    category?: Category[]
    loop?: number
    videoTypeId?: number
    videoType?: VideoType
    noBackToBack?: Category[]
    doNotPlay?: Building[]
    subCategory: Category[]
}
