import { useEffect, useState, type SyntheticEvent } from "react"
import { Link, useNavigate, useParams } from "react-router-dom"
import { useAuth } from "../context/AuthContext"
import { editAddress, getAddress } from "../api/AddressesApi"
import type { AdresaEditRequest } from "../types/Adresa"

function EditAddressPage() {
    const { accessToken } = useAuth()
    const { id } = useParams<{ id: string }>()
    const navigate = useNavigate()

    const [formular, setFormular] = useState<AdresaEditRequest>({
        numeDestinatar: "",
        strada: "",
        oras: "",
        judet: "",
        codPostal: "",
        tara: "",
        numarTelefon: "",
        estePrincipala: false,
    })

    const [isLoading, setIsLoading] = useState(true)
    const [isSaving, setIsSaving] = useState(false)
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        async function incarcaAdresa() {
            if (!accessToken) {
                setError("Nu esti autentificat")
                setIsLoading(false)
                return
            }

            if (!id) {
                setError("ID-ul adresei lipseste")
                setIsLoading(false)
                return
            }

            const idAdresa = Number(id)

            if (Number.isNaN(idAdresa)) {
                setError("ID-ul adresei este invalid")
                setIsLoading(false)
                return
            }

            try {
                setIsLoading(true)
                setError(null)

                const adresa = await getAddress(idAdresa, accessToken)

                setFormular({
                    numeDestinatar: adresa.numeDestinatar,
                    strada: adresa.strada,
                    oras: adresa.oras,
                    judet: adresa.judet,
                    codPostal: adresa.codPostal,
                    tara: adresa.tara,
                    numarTelefon: adresa.numarTelefon,
                    estePrincipala: adresa.estePrincipala,
                })
            } catch (error: unknown) {
                if (error instanceof Error) {
                    setError(error.message)
                } else {
                    setError("A aparut o eroare necunoscuta")
                }
            } finally {
                setIsLoading(false)
            }
        }

        incarcaAdresa()
    }, [accessToken, id])

    function handleChange(
        event: React.ChangeEvent<HTMLInputElement>
    ) {
        const { name, value, type, checked } = event.target

        setFormular((formularCurent) => ({
            ...formularCurent,
            [name]: type === "checkbox" ? checked : value,
        }))
    }

    async function handleSubmit(event: SyntheticEvent<HTMLFormElement>) {
        event.preventDefault()

        if (!accessToken) {
            setError("Nu esti autentificat")
            return
        }

        if (!id) {
            setError("ID-ul adresei lipseste")
            return
        }

        const idAdresa = Number(id)

        if (Number.isNaN(idAdresa)) {
            setError("ID-ul adresei este invalid")
            return
        }

        try {
            setIsSaving(true)
            setError(null)

            await editAddress(
                formular,
                idAdresa,
                accessToken
            )

            navigate("/addresses")
        } catch (error: unknown) {
            if (error instanceof Error) {
                setError(error.message)
            } else {
                setError("A aparut o eroare necunoscuta")
            }
        } finally {
            setIsSaving(false)
        }
    }

    if (isLoading) {
        return <p>Se incarca adresa...</p>
    }

    return (
        <div>
            <h1>Editeaza adresa</h1>

            {error && <p>{error}</p>}

            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="numeDestinatar">
                        Nume destinatar
                    </label>

                    <input
                        id="numeDestinatar"
                        name="numeDestinatar"
                        value={formular.numeDestinatar}
                        onChange={handleChange}
                    />
                </div>

                <div>
                    <label htmlFor="strada">
                        Strada
                    </label>

                    <input
                        id="strada"
                        name="strada"
                        value={formular.strada}
                        onChange={handleChange}
                    />
                </div>

                <div>
                    <label htmlFor="oras">
                        Oras
                    </label>

                    <input
                        id="oras"
                        name="oras"
                        value={formular.oras}
                        onChange={handleChange}
                    />
                </div>

                <div>
                    <label htmlFor="judet">
                        Judet
                    </label>

                    <input
                        id="judet"
                        name="judet"
                        value={formular.judet}
                        onChange={handleChange}
                    />
                </div>

                <div>
                    <label htmlFor="codPostal">
                        Cod postal
                    </label>

                    <input
                        id="codPostal"
                        name="codPostal"
                        value={formular.codPostal}
                        onChange={handleChange}
                    />
                </div>

                <div>
                    <label htmlFor="tara">
                        Tara
                    </label>

                    <input
                        id="tara"
                        name="tara"
                        value={formular.tara}
                        onChange={handleChange}
                    />
                </div>

                <div>
                    <label htmlFor="numarTelefon">
                        Numar telefon
                    </label>

                    <input
                        id="numarTelefon"
                        name="numarTelefon"
                        value={formular.numarTelefon}
                        onChange={handleChange}
                    />
                </div>

                <div>
                    <label htmlFor="estePrincipala">
                        <input
                            id="estePrincipala"
                            name="estePrincipala"
                            type="checkbox"
                            checked={formular.estePrincipala}
                            onChange={handleChange}
                        />

                        Adresa principala
                    </label>
                </div>

                <button
                    type="submit"
                    disabled={isSaving}
                >
                    {isSaving ? "Se salveaza..." : "Salveaza"}
                </button>
            </form>

            <Link to="/addresses">
                Anuleaza
            </Link>
        </div>
    )
}

export default EditAddressPage