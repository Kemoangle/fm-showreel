import { Restriction } from "./restriction"

export interface Building{
    id?: number
    buildingName?: string
    address?: string
    district?: string
    remark?: string
    createTime?: Date
    lastUpdateTime?: Date
    zone?: string
    postalCode?: number
    restrictions?: Restriction[]
}

