import { Category } from "./category";
import { VideoType } from "./videoType";

export interface BuildingRestriction{
    id?: number
    buildingId?: number
    name?: string
    type?: string
    categoryId?: number
    except?: VideoType[]
    category?: Category | null
}
