import { ApiError } from "./ApiError";
import type { Adresa } from "../types/Adresa";

const API_URL = import.meta.env.VITE_API_URL

export async function getAdresses(
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