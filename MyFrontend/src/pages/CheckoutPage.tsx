import { useEffect, useState } from "react"
import { Link } from 'react-router-dom'
import { getAddresses } from "../api/AddressesApi"
import { useAuth } from "../context/AuthContext"
import { useCart } from "../context/CartContext"
import { createOrder } from "../api/OrdersApi"
import type { Adresa } from "../types/Adresa"
import type { Comanda, ComandaCreateRequest} from "../types/Comanda"

function CheckoutPage() {
    const { accessToken } = useAuth()
    const {items, golesteCos} = useCart()

    const [adrese, setAdrese] = useState<Adresa[]>([])
    const [adresaSelectataId, setAdresaSelectataId] = useState<number | null>(null)
    const [isSubmitting, setIsSubmitting] = useState(false)
    const [submitError, setSubmitError] = useState<string | null>(null)
    const [comandaConfirmata, setComandaConfirmata] = useState<Comanda | null>(null)
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

                const adreseIncarcate = await getAddresses(accessToken)
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

    async function handlePlaceOrder() {
        if(!accessToken) {
            setSubmitError('Nu esti autentificat')
            return
        }

        if(adresaSelectataId === null) {
            setSubmitError('Alege o adresa de livrare')
            return
        }

        if(items.length === 0) {
            setSubmitError('Cosul este gol')
            return
        }

        const comandaNoua: ComandaCreateRequest = {
            adresaId: adresaSelectataId,
            elementeComandaCreateDto: items.map((item) => ({
                produsId: item.produs.id,
                cantitate: item.cantitate,
            }))
        }

        try{
            setIsSubmitting(true)
            setSubmitError(null) 
            
            const comandaCreata = await createOrder(
                comandaNoua,
                accessToken
            )

            setComandaConfirmata(comandaCreata)
            golesteCos()
        } catch(error: unknown) {
            if(error instanceof Error) {
                setSubmitError(error.message)
            } else {
                setSubmitError('A aparut o eroare necunoscuta')
            }
        } finally {
            setIsSubmitting(false)
        }
    }

    if(isLoading) {
    return <p>Se incarca Adresele</p>
    }

    if(error) {
        return <p>{error}</p>
    }

    if(adrese.length === 0) {
        return(
            <div>
                <h1>Nu ai nicio adresa de livrare</h1>

                <Link to="/addAddress">Adauga o adresa</Link>
            </div>
        )
    }
    
    if(comandaConfirmata) {
        return(
            <div>
                <h1>Comanda a fost plasata</h1>

                <p>Numar comanda: {comandaConfirmata.id}</p>
                <p>Status: {comandaConfirmata.status}</p>
                <p>Total: {comandaConfirmata.total}</p>

                <Link to="/products">Continua cumparaturile</Link>
            </div>
        )
    }

    return (
        <div>
            <h1>Finalizeaza comanda</h1>
            
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
            
            {submitError && <p>{submitError}</p>}
            <Link to="/addresses/new">Adauga adresa</Link>
            <button
                type="button"
                onClick={handlePlaceOrder}
                disabled={
                    isSubmitting ||  items.length === 0 || !adresaSelectataId
                }
            >
                {isSubmitting ? 'Se plaseaza comanda..' : 'Plaseaza comanda'}
            </button>


        </div>
    )
}

export default CheckoutPage