export interface Categorie {
    id: number
    nume: string | null
    rowVersion: string
}

export interface CategorieFiltru {
    nume?: string
    descending?: boolean
}

export interface CategorieRequest {
    nume: string
}

export interface CategorieUpdate {
    nume: string
    rowVersion: string
}