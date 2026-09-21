import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { deleteAddress, getAddresses } from '../api/AddressesApi'
import { useAuth } from '../context/AuthContext'
import type { Adresa } from '../types/Adresa'

function AddressesPage() {
    const { accessToken } = useAuth()

    const [adrese, setAdrese] = useState<Adresa[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)
    const [idInStergere, setIdInStergere] = useState<number | null>(null)

    useEffect(() => {
        async function incarcaAdresele() {
            if (!accessToken) {
                setError('Nu esti autentificat')
                setIsLoading(false)
                return
            }

            try {
                setIsLoading(true)
                setError(null)

                const adreseIncarcate = await getAddresses(accessToken)
                setAdrese(adreseIncarcate)
            } catch (error: unknown) {
                if (error instanceof Error) {
                    setError(error.message)
                } else {
                    setError('A aparut o eroare necunoscuta')
                }
            } finally {
                setIsLoading(false)
            }
        }

        incarcaAdresele()
    }, [accessToken])

    async function handleSterge(id: number) {
        if (!accessToken) {
            return
        }

        const confirmat = window.confirm('Esti sigur ca vrei sa stergi aceasta adresa?')
        if (!confirmat) {
            return
        }

        try {
            setIdInStergere(id)
            await deleteAddress(id, accessToken)

            setAdrese((adreseCurente) =>
                adreseCurente.filter((adresa) => adresa.id !== id)
            )
        } catch (error: unknown) {
            if (error instanceof Error) {
                setError(error.message)
            } else {
                setError('A aparut o eroare necunoscuta')
            }
        } finally {
            setIdInStergere(null)
        }
    }

    if (isLoading) {
        return <p>Se incarca adresele...</p>
    }

    return (
        <div>
            <h1>Adresele mele</h1>

            {error && <p>{error}</p>}

            <Link to="/addresses/new">Adauga o adresa noua</Link>

            {adrese.length === 0 ? (
                <p>Nu ai nicio adresa salvata.</p>
            ) : (
                <ul>
                    {adrese.map((adresa) => (
                        <li key={adresa.id}>
                            <p>
                                {adresa.numeDestinatar} — {adresa.strada}, {adresa.oras},{' '}
                                {adresa.judet}, {adresa.tara}
                                {adresa.estePrincipala && ' (principala)'}
                            </p>

                            <Link to={`/addresses/${adresa.id}/edit`}>Editeaza</Link>

                            <button
                                type="button"
                                onClick={() => handleSterge(adresa.id)}
                                disabled={idInStergere === adresa.id}
                            >
                                {idInStergere === adresa.id ? 'Se sterge...' : 'Sterge'}
                            </button>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    )
}

export default AddressesPage