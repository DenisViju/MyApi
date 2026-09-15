import { createContext, useContext, useState, type ReactNode } from "react";
import { login } from "../api/AuthApi";
import type { LoginRequest, User } from "../types/Auth";

type AuthContextType = {
    user: User | null
    accessToken: string | null
    autentifica: (dateLogin: LoginRequest) => Promise<void>
    delogheaza: () => void
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

export function AuthProvider({children}: {children: ReactNode}) {
    const [user, setUser] = useState<User | null>(null)
    const [accessToken, setAccesToken] = useState<string | null>(null)

    async function autentifica(dateLogin: LoginRequest) {
        const raspuns = await login(dateLogin)

        setUser(raspuns.userDto)
        setAccesToken(raspuns.accesToken)
    }

    function delogheaza() {
        setUser(null)
        setAccesToken(null)
    }

    return (
        <AuthContext.Provider
            value = {{user, accessToken, autentifica, delogheaza}}
        >
            {children}
        </AuthContext.Provider>
    )
}

export function useAuth() {
    const auth = useContext(AuthContext)

    if(!auth) {
        throw new Error(`useAuth trebuie folosit in interiorul AuthProvider`)
    }

    return auth
}
