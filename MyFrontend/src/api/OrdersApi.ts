import type { Comanda, ComandaCreateRequest } from "../types/Comanda";
import { ApiError } from "./ApiError";


export async function createOrder(
    comandaNoua: ComandaCreateRequest,
    accessToken: string
): Promise<Comanda> {
    const API_URL = import.meta.env.VITE_API_URL

    const result = await fetch(`${API_URL}/api/comanda`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
             Authorization: `Bearer ${accessToken}`,
        },
        body: JSON.stringify(comandaNoua),
    })


    if(!result.ok) {
        const problemDetails = await result.json()

        throw new ApiError(
            result.status, 
            problemDetails.title ?? 'Comanda nu a putut fi creata')
    }

    return result.json()
}

