export interface LoginRequest {
    username: string
    password: string
}

export interface User {
    id: number
    username: string | null
    role: string | null
}

export interface LoginResponse {
    userDto: User
    accessToken: string
}