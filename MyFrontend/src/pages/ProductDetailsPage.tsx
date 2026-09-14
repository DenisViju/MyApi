import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { getProduct } from '../api/ProductsApi'
import type { Produs } from '../types/Produs'
import { useCart } from '../context/CartContext'

function ProductDetailsPage() {
    const { id } = useParams()
    const { adaugaProdus } = useCart()
    const [produs, setProdus] = useState<Produs | null>(null)
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)

    useEffect( () => {
        async function LoadProduct() {
            if(!id) {
                setError("Lipseste id-ul produsului")
                setIsLoading(false)
                return
            }

            try {
                setIsLoading(true)
                setError(null)

                const produsIncarcat = await getProduct(Number(id))
                setProdus(produsIncarcat)

            } catch(error: unknown) {
                if(error instanceof Error) {
                    setError(error.message)
                }else {
                    setError("A aparut o eroare necunoscuta")
                }
                
            } finally {
                setIsLoading(false)
            }

        }
        LoadProduct()
    }, [id])
    
if(isLoading) {
    return <p>Se incarca Produsul</p>
}
if(error) {
    return <p>{error}</p>
}
if(!produs) {
    return <p>Produsul nu a fost gasit</p>
}

function handleAdaugaInCos() {
  adaugaProdus(produs!)
}

return (
    <div>
        <Link to={`/products`}>Inapoi la produse</Link>

        <h1>{produs.nume}</h1>
        <p>{produs.descriere}</p>
        <p>Categorie: {produs.numeCategorie}</p>
        <p>Preț: {produs.pret} lei</p>
        <p>Stoc: {produs.stoc}</p>

        <button
            type="button"
            onClick={handleAdaugaInCos}
            disabled={produs.stoc === 0}
            >
            Adauga in cos
        </button>
        
    </div>
)
}

export default ProductDetailsPage
