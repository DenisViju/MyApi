import type { Comanda, ComandaCreateRequest } from "../types/Comanda";
import { ApiError } from "./ApiError";


const API_URL = import.meta.env.VITE_API_URL

export async function createOrder(
    comandaNoua: ComandaCreateRequest,
    accessToken: string
): Promise<Comanda> {

    const response = await fetch(`${API_URL}/api/Comanda`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
             Authorization: `Bearer ${accessToken}`,
        },
        body: JSON.stringify(comandaNoua),
    })


    if(!response.ok) {
        const problemDetails = await response.json()

        throw new ApiError(
            response.status, 
            problemDetails.title ?? 'Comanda nu a putut fi creata')
    }

    return response.json()
}

export async function getOrders(accessToken: string) : Promise<Comanda[]> {
    const response = await fetch(`${API_URL}/api/Comanda`, {
        headers: {
            Authorization: `Bearer ${accessToken}`,
        }
    })

    if(!response.ok) {
        const problemDetails = await response.json()

        throw new ApiError(
            response.status,
            problemDetails.title ?? 'Comenzile nu au putut fi incarcate'
        )
    }

    return response.json()
}

