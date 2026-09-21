import { ApiError } from "./ApiError";
import type { Adresa, AdresaCreateRequest, AdresaEditRequest } from "../types/Adresa";

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

export async function deleteAddress(
    id: number,
    accessToken: string,
) : Promise<void> {
    const response = await fetch(`${API_URL}/api/Adresa/${id}`, {
        method: 'DELETE',
        headers: {
            Authorization: `Bearer ${accessToken}`
        }
    })
    if(!response.ok) {
        const problemDetails = await response.json()

        throw new ApiError(
            response.status,
            problemDetails.title ?? 'Adresa nu a putut fi stearsa'
        )
    }

}

export async function editAddress(
    adresaEditata: AdresaEditRequest,
    id: number,
    accessToken: string
) : Promise<Adresa> {
    const response = await fetch(`${API_URL}/api/Adresa/${id}`, {
        method: 'PUT',
        headers: {
            'content-type': 'application/json',
            Authorization: `Bearer ${accessToken}`,
        },
        body: JSON.stringify(adresaEditata)
    })

    if(!response.ok) {
        const problemDetails = await response.json()

        throw new ApiError(
            response.status,
            problemDetails.title ?? 'Adresa nu a putut fi editata'
        )
    }

    return response.json()
}

export async function getAddress (
    id: number,
    accessToken: string
) : Promise<Adresa> {
    const response = await fetch(`${API_URL}/api/Adresa/${id}`, {
        headers: {
            Authorization: `Bearer ${accessToken}`
        }
    })

    if(!response.ok) {
        const problemDetails = await response.json()

        throw new ApiError(
            response.status,
            problemDetails.title ?? 'Adresa nu a putut fi incarcata'
        )
    }

    return response.json()
}