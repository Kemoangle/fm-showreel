import { Building } from "./building";
import { VideoType } from "./videoType";

export interface BuildingRestriction{
    id?: number
    buildingId?: Building
    name?: string
    type?: string
    categoryId?: number
    except?: VideoType[]
}
