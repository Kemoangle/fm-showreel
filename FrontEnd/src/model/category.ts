
export interface Category{
    id: number
    name?: string
    parent?: number
    subCategory?: Category[] 
}

export interface SubCategory{
    id: number
    name?: string
    parent?: number
}
