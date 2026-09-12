import type {Produs} from "../types/Produs"
import type { PagedResult } from '../types/PagedResult'
import { ApiError } from "./ApiError"

const API_URL = import.meta.env.VITE_API_URL

export async function getProducts(
    page: number,
    pageSize: number
):Promise<PagedResult<Produs>> {
    const response = await fetch(
        `${API_URL}/api/Produse?page=${page}&pageSize=${pageSize}`)
    

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
        `${API_URL}/api/Produs/${id}`
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