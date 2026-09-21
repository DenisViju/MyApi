import { useState, type SyntheticEvent } from "react"
import { useNavigate, Link } from "react-router-dom"
import { register } from "../api/AuthApi"
import { useAuth } from "../context/AuthContext"

function RegisterPage() {
    const { autentifica } = useAuth()
    const navigate = useNavigate()
    
    const [username, setUsername] = useState("")
    const [password, setPassword] = useState("")
    const [confirmPassword, setConfirmPassword] = useState("")
    const [error, setError] = useState<string | null>(null)
    const [isSubmitting, setIsSubmitting] = useState(false)

    async function handleSubmit(event: SyntheticEvent<HTMLFormElement>) {
        event.preventDefault()
        if(password !== confirmPassword) {
            setError('Parolele nu coincid')
            return
        }

        try {
            setIsSubmitting(true)
            setError(null)

            await register({username, password})
            await autentifica({ username, password })

            navigate('/products')
        } catch(error: unknown) {
            if(error instanceof Error) {
                setError(error.message)
            } else {
                setError('A aparut o eroare necunoscuta')
            }
        } finally {
            setIsSubmitting(false)
        }
    }
    return(
        <div>
            <h1>Inregistrare</h1>

            <form onSubmit={handleSubmit}>
                <label>
                    Nume utilizator
                    <input 
                        type="text"
                        value = {username}
                        onChange={(event) => setUsername(event.target.value)} 
                        autoComplete="username"
                        required
                    />
                </label>

                <label>
                    Parola
                    <input 
                        type="password"
                        value = {password}
                        onChange={(event) => setPassword(event.target.value)} 
                        autoComplete="new-password"
                        minLength={4}
                        required
                    />
                </label>

                <label>
                    Confirma Parola
                    <input 
                        type="password"
                        value = {confirmPassword}
                        onChange={(event) => setConfirmPassword(event.target.value)} 
                        autoComplete="new-password"
                        minLength={4}
                        required
                    />
                </label>

                {error && <p>{error}</p>}

                <button type="submit" disabled={isSubmitting}>
                    {isSubmitting ? 'Se inregistreaza...' : 'Creeaza cont'}
                </button>
            </form>

            <p>
                Ai deja cont? <Link to="/login">Autentifica-te</Link>
            </p>

        </div>
    )
}

export default RegisterPage