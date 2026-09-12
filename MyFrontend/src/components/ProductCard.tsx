import type { Produs } from "../types/Produs"

type ProductCardProps = {
    produs: Produs
}

function ProductCard({produs} : ProductCardProps) {

    return (
        <div>
            <h2>{produs.nume}</h2>
            <p>{produs.descriere}</p>
            <p>{produs.pret} lei</p>
            <p>Stoc: {produs.stoc}</p>
        </div>
    )
}

export default ProductCard