import { useState } from "react"
import { Link } from "react-router-dom"
import { createAddress } from "../api/AddressesApi"
import { useAuth } from "../context/AuthContext"
import type { SyntheticEvent } from "react"
import type { Adresa, AdresaCreateRequest } from "../types/Adresa"


function AddAddressPage() {
    const {accessToken} = useAuth()

    const [adresaConfirmata, setAdresaConfirmata] = useState<Adresa | null>(null)
    const [numeDestinatar, setNumeDestinatar] = useState('')
    const [strada, setStrada] = useState('')
    const [oras, setOras] = useState('')
    const [judet, setJudet] = useState('')
    const [codPostal, setCodPostal] = useState('')
    const [tara, setTara] = useState('')
    const [numarTelefon, setNumarTelefon] = useState('')
    const [estePrincipala, setEstePrincipala] = useState(false)
    const [error, setError] = useState<string | null>(null)
    const [isSubmitting, setIsSubmitting] = useState(false)

    async function handleCreateAddress(event: SyntheticEvent<HTMLFormElement>) {
        event.preventDefault()
        if(!accessToken) {
                setError('Nu esti autentificat')
                setIsSubmitting(false)
                return
            }

        try {
            setIsSubmitting(true)
            setError(null)

            const adresaNoua: AdresaCreateRequest = {
                numeDestinatar: numeDestinatar,
                strada: strada,
                oras: oras,
                judet: judet,
                codPostal: codPostal,
                tara: tara,
                numarTelefon: numarTelefon,
                estePrincipala: estePrincipala
            }
            const adresaCreata = await createAddress(adresaNoua, accessToken)
            setAdresaConfirmata(adresaCreata)

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

    if(adresaConfirmata) {
        return(
            <div>
                <h1>Adresa a fost creata cu succes</h1>

                <p>Nume destinatar: {adresaConfirmata.numeDestinatar}</p>
                <p>Strada: {adresaConfirmata.strada}</p>
                <p>Oras: {adresaConfirmata.oras}</p>
                <p>Judet: {adresaConfirmata.judet}</p>
                <p>Cod postal: {adresaConfirmata.codPostal}</p>
                <p>Tara: {adresaConfirmata.tara}</p>
                <p>Numar de telefon: {adresaConfirmata.numarTelefon}</p>
                
                {adresaConfirmata.estePrincipala 
                ? <p>Adresa este principala</p> 
                : <p>Adresa nu este principala</p>}

                <Link to="/checkout">Continua finalizarea comenzii</Link>
            </div>
        )

    }

    return(
        <div>
            <h1>Adauga o noua adresa</h1>

            <form onSubmit={handleCreateAddress}>
                <label>
                    Nume destinatar
                    <input 
                        type="text"
                        value={numeDestinatar}
                        onChange={(event) => setNumeDestinatar(event.target.value)}
                        required
                    />
                </label>
                <label>
                    Strada
                    <input 
                        type="text"
                        value={strada}
                        onChange={(event) => setStrada(event.target.value)}
                        required
                     />
                </label>

                <label>
                    Oras
                    <input 
                        type="text"
                        value={oras}
                        onChange={(event) => setOras(event.target.value)}
                        required
                    />
                </label>

                <label>
                    Judet
                    <input
                        type="text"
                        value={judet}
                        onChange={(event) => setJudet(event.target.value)}
                        required
                    />
                </label>

                <label>
                    Cod Postal
                    <input 
                        type="text" 
                        value={codPostal}
                        onChange={(event) => setCodPostal(event.target.value)}
                        required
                    />
                </label>

                <label>
                    Tara
                    <input 
                        type="text" 
                        value={tara}
                        onChange={(event) => setTara(event.target.value)}
                        required
                    />
                </label>

                <label>
                    Numar de telefon
                    <input 
                        type="text" 
                        value={numarTelefon}
                        onChange={(event) => setNumarTelefon(event.target.value)}
                        required
                    />
                </label>

                <label>
                    Marcheaza ca adresa principala
                    <input 
                        type="checkbox"
                        checked={estePrincipala}
                        onChange={(event) => setEstePrincipala(event.target.checked)}
                     />
                </label>

                {error && <p>{error}</p>}

                <button 
                    type="submit"
                    disabled={isSubmitting}
                >
                    {isSubmitting ? 'Se adauga adresa..' : 'Adauga adresa'}
                </button>


            </form>
        </div>
    )
}

export default AddAddressPage