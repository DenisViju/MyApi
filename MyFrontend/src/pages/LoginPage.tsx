import { useState, type SyntheticEvent } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

function LoginPage() {
    const { autentifica } = useAuth()
    const  navigate = useNavigate()

    const [username, setUsername] = useState('')
    const [password, setPassword] = useState('')
    const [error, setError] = useState<string | null>(null)
    const [isSubmitting, setIsSubmitting] = useState(false)
    
    async function handleSubmit(event: SyntheticEvent<HTMLFormElement>) {
        event.preventDefault()

        try {
            setIsSubmitting(true)
            setError(null)

            await autentifica({username, password})
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
            <h1>Autentificare</h1>

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
                        autoComplete="current-password"
                        required
                    />
                </label>

                {error && <p>{error}</p>}

                <button type="submit" disabled={isSubmitting}>
                    {isSubmitting ? 'Se autentifica' : 'Autentificare'}
                </button>

                <p>
                    Nu ai cont? <Link to="/register">Inregistreaza-te</Link>
                </p>
            </form>
        </div>
    )

}

export default LoginPage