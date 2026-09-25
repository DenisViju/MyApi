import type { Categorie, CategorieFiltru } from "../types/Categorie"
import type { PagedResult } from "../types/PagedResult"
import { ApiError } from "./ApiError"

const API_URL = import.meta.env.VITE_API_URL

export async function getCategories(
    page: number,   
    pageSize: number,
    filtru: CategorieFiltru
) : Promise<PagedResult<Categorie>> {
    const params = new URLSearchParams({
        page: page.toString(),
        pageSize: pageSize.toString()
    })

    Object.entries(filtru).forEach(([key, value]) => {
        if(value !== undefined && value !== null && value !=='') {
            params.append(key, value.toString())
        }
    })

    const response = await fetch(`${API_URL}/api/Categorii?${params}`)

    if(!response.ok) {
        const problemDetails = await response.json()

        throw new ApiError(
            response.status, 
            problemDetails.title ?? 'A aparut o eroare'
        )
    }

    return response.json()
}