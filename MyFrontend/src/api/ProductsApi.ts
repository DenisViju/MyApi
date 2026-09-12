import type {Produs} from "../types/Produs"
import type { PagedResult } from '../types/PagedResult'
import type {ProdusFiltru} from '../types/ProdusFiltru'
import { ApiError } from "./ApiError"

const API_URL = import.meta.env.VITE_API_URL

export async function getProducts(
    page: number,
    pageSize: number,
    filtru: ProdusFiltru
):Promise<PagedResult<Produs>> {
    const params = new URLSearchParams({
        page: page.toString(),
        pageSize: pageSize.toString()
    })

    Object.entries(filtru).forEach(([key, value]) => {
        if (value !== undefined && value !== null && value !== '') {
            params.append(key, value.toString())
        }
    })

    const response = await fetch(
        `${API_URL}/api/Produse?${params}`)
    

    if(!response.ok) {       
        const problemDetails = await response.json()

        throw new ApiError(
            response.status, 
            problemDetails.title ?? 'A aparut o eroare')
    }

    return response.json()
}

export async function getProduct(
    id: number
):Promise<Produs> {
    const response = await fetch(
        `${API_URL}/api/Produse/${id}`
    )
    if(!response.ok) {
        const problemDetails = await response.json()

        throw new ApiError(
            response.status,
            problemDetails.title ?? 'A aparut o eroare'
        )
    }
    return response.json()
}