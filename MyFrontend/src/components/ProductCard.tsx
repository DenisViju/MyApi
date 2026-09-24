import { useState } from "react"
import type { Produs } from "../types/Produs"
import { Link } from "react-router-dom"
import { useCart } from "../context/CartContext"
import QuantitySelector from "./QuantitySelector"

type ProductCardProps = {
    produs: Produs
}

function ProductCard({ produs }: ProductCardProps) {
    const { adaugaProdus } = useCart()
    const [cantitate, setCantitate] = useState(1)

    function handleAdaugaInCos() {
        adaugaProdus(produs, cantitate)
        setCantitate(1)
    }

    return (
        <div>
            <h2>{produs.nume}</h2>

            <p>{produs.descriere}</p>

            <p>
                Categorie: {produs.numeCategorie}
            </p>

            <p>{produs.pret} lei</p>

            <p>Stoc: {produs.stoc}</p>

            <Link to={`/products/${produs.id}`}>
                Vezi detalii
            </Link>

            <br />

            {produs.stoc > 0 && (
                <QuantitySelector
                    cantitate={cantitate}
                    stoc={produs.stoc}
                    onChange={setCantitate}
                />
            )}

            <br />

            <button
                type="button"
                onClick={handleAdaugaInCos}
                disabled={produs.stoc === 0}
            >
                {produs.stoc === 0
                    ? "Stoc epuizat"
                    : "Adauga in cos"}
            </button>
        </div>
    )
}

export default ProductCard
