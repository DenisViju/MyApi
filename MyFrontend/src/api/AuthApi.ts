import { ApiError } from "./ApiError";
import type { LoginRequest, LoginResponse } from "../types/Auth";

const API_URL = import.meta.env.VITE_API_URL

export async function login(
    dateLogin: LoginRequest
): Promise<LoginResponse> {
    const response = await fetch(`${API_URL}/api/Auth/login`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        credentials: 'include',
        body: JSON.stringify(dateLogin)

    })

    if(!response.ok) {
        const problemDetails = await response.json()

        throw new ApiError(
            response.status,
            problemDetails.title ?? 'Autentificarea a esuat'
        )
    }

    return response.json()
    
}

export async function refresh() : Promise<LoginResponse> {
    const response = await fetch(`${API_URL}/api/Auth/refresh`, {
        method: 'POST',
        credentials: 'include'
    })

    if(!response.ok) {
        const problemDetails = await response.json()

        throw new ApiError(
            response.status,
            problemDetails.title ?? 'Sesiunea a expirat'
        )
    }

    return response.json()
}

export async function logout() : Promise<void> {
    await fetch(`${API_URL}/api/Auth/logout`, {
        method: `POST`,
        credentials: 'include'
    })
}

