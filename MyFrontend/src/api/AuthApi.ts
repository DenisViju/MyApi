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