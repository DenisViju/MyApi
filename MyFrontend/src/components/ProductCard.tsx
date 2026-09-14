import type { Produs } from "../types/Produs"
import { Link } from 'react-router-dom'

type ProductCardProps = {
    produs: Produs
}

function ProductCard({ produs }: ProductCardProps) {
    return (
        <div>
            <h2>{produs.nume}</h2>
            <p>{produs.descriere}</p>
            <p>Categorie: {produs.numeCategorie}</p>
            <p>{produs.pret} lei</p>
            <p>Stoc: {produs.stoc}</p>
            <Link to={`/products/${produs.id}`}>Vezi detalii</Link>
        </div>
    )
}

export default ProductCard

