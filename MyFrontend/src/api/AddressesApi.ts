import { ApiError } from "./ApiError";
import type { Adresa, AdresaCreateRequest } from "../types/Adresa";

const API_URL = import.meta.env.VITE_API_URL

export async function getAddresses(
    accessToken: string
): Promise<Adresa[]> {
    const response = await fetch(`${API_URL}/api/Adresa/adrese`, {
        headers: {
            Authorization: `Bearer ${accessToken}`,
        },
    })

    if(!response.ok) {
        const problemDetails = await response.json()

        throw new ApiError(
            response.status,
            problemDetails.title ?? 'Nu s-au putut incarca adresele'
        )
    }
    return response.json()
}

export async function createAddress(
    adresaNoua: AdresaCreateRequest,
    accessToken: string,
) :Promise<Adresa> {
    const response = await fetch(`${API_URL}/api/Adresa`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${accessToken}`
        },
        body: JSON.stringify(adresaNoua),
    })

    if(!response.ok) {
        const problemDetails = await response.json()

        throw new ApiError(
            response.status,
            problemDetails.title ?? 'Adresa nu a putut fi creata'
        )
    }

    return response.json()
}