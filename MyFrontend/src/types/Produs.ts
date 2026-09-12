export interface Produs {
    id: number
    nume: string | null
    descriere: string | null
    pret: number 
    stoc: number
    categorieId: number
    numeCategorie: string | null
    rowVersion: string
}