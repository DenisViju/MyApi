import { useEffect, useState } from "react"
import { getAdresses } from "../api/AdressesApi"
import { useAuth } from "../context/AuthContext"
import type { Adresa } from "../types/Adresa"

function CheckoutPage() {
    const { accessToken } = useAuth()

    const [adrese, setAdrese] = useState<Adresa[]>([])
    const [adresaSelectataId, setAdresaSelectataId] = useState<number | null>(null)
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)


    useEffect(() => {
        async function loadAdresses() {
            if(!accessToken) {
                setError('Nu esti autentificat')
                setIsLoading(false)
                return
            }

            try{
                setIsLoading(true)
                setError(null)

                const adreseIncarcate = await getAdresses(accessToken)
                setAdrese(adreseIncarcate)
                const adresaInitiala = 
                    adreseIncarcate.find((adresa) => adresa.estePrincipala) ??
                    adreseIncarcate[0]

                setAdresaSelectataId(adresaInitiala?.id ?? null)
            } catch(error: unknown) {
                if(error instanceof Error) {
                    setError(error.message)
                } else {
                    setError('A aparut o eroare necunoscuta')
                }
            } finally {
                setIsLoading(false)
            }
        }
        loadAdresses()
    } ,[accessToken])

    if(isLoading) {
    return <p>Se incarca Adresele</p>
    }

    if(error) {
        return <p>{error}</p>
    }

    return (
        <div>
            <h1>Finalizeaza comanda</h1>
            {adrese.length === 0 ? (
                <p>Nu ai inca o adresa de livrare</p>
            ) : (
                <fieldset>
                    <legend>Alege adresa de livrare</legend>

                    {adrese.map((adresa) => (
                        <label key={adresa.id}>
                            <input
                                 type="radio" 
                                 name="adresa"
                                 checked={adresaSelectataId === adresa.id}
                                 onChange={() => setAdresaSelectataId(adresa.id)}
                            />

                            <span>
                                {adresa.numeDestinatar} — {adresa.strada}, {adresa.oras},
                                {' '}{adresa.judet}, {adresa.tara}
                            </span>
                        </label>
                    ))}
                </fieldset>
            )}
        </div>
    )
}

export default CheckoutPage