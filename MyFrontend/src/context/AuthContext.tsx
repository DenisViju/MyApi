import { createContext, useContext, useState, useEffect, useRef, type ReactNode } from "react";
import { login, logout, refresh } from "../api/AuthApi";
import type { LoginRequest, User } from "../types/Auth";

type AuthContextType = {
    user: User | null
    accessToken: string | null
    isLoading: boolean
    autentifica: (dateLogin: LoginRequest) => Promise<void>
    delogheaza: () => void
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

export function AuthProvider({children}: {children: ReactNode}) {
    const [user, setUser] = useState<User | null>(null)
    const [accessToken, setAccessToken] = useState<string | null>(null)
    const [isLoading, setIsLoading] = useState(true)
    const refreshPornit = useRef(false)

    useEffect(() => {
         if (refreshPornit.current) {
            return
        }
        refreshPornit.current = true
        async function incearcaRefresh() {
            try{
                const raspuns = await refresh()
                setUser(raspuns.userDto)
                setAccessToken(raspuns.accessToken)
            } catch {
                setUser(null)
                setAccessToken(null)
            } finally {
                setIsLoading(false)
            }
        }
        incearcaRefresh()
    }, [])

    async function autentifica(dateLogin: LoginRequest) {
        const raspuns = await login(dateLogin)

        setUser(raspuns.userDto)
        setAccessToken(raspuns.accessToken)
    }

    async function delogheaza() {
        try {
           await logout()
        } finally  {
            setUser(null)
            setAccessToken(null)
        }
        
    }

    return (
        <AuthContext.Provider
            value = {{user, accessToken, isLoading, autentifica, delogheaza}}
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
