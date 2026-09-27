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
       <article className="product-card">

            <Link
                className="product-card-image"
                to={`/products/${produs.id}`}
            >
                <div>
                    Produs
                </div>
            </Link>

            <div className="product-card-info">
                
                <p className="product-card-category">
                    {produs.numeCategorie}
                </p>

                <h2 className="product-card-name">
                    {produs.nume}
                </h2>

                <p className="product-card-description">
                    {produs.descriere}
                </p>

                <p className="product-card-price">
                    {produs.pret} lei
                </p>

                <p className="product-card-stock">
                    {produs.stoc > 0 
                        ?  `In stoc: ${produs.stoc}`
                        : "Stoc epuizat"}
                </p>
            </div>

            <div className="product-card-actions">

                {produs.stoc > 0 && (
                    <QuantitySelector 
                        cantitate={cantitate}
                        stoc={produs.stoc}
                        onChange={setCantitate}
                    />
                )}

                <button
                    type="button"
                    onClick={handleAdaugaInCos}
                    disabled={produs.stoc === 0}
                    className="product-card-button"
                >
                    {produs.stoc === 0 
                        ? "Stoc epuizat"
                        : "Adauga in cos"}
                </button>

            </div>

       </article>
    )
}

export default ProductCard
