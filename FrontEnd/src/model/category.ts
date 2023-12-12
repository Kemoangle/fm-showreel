
export interface Category{
    id: number
    name?: string
    parent?: number
    subCategory?: SubCategory[] 
}

export interface SubCategory{
    id: number
    name?: string
    parent?: number
}
