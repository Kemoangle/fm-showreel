import { Category } from "./category"

export interface Restriction{
    id?: number
    buildingId?: number
    categoryId?: number
    type?: string
    category?: Category
    arrCategory?: Category[]
    arrItem?: Category[]

}
