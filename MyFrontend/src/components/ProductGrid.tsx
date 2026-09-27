import type { Produs } from "../types/Produs"
import ProductCard from "./ProductCard"

type ProductGridProps = {
    products: Produs[]
}

function ProductGrid({ products }: ProductGridProps) {
    return (
        <div className="product-grid">
            {products.map((product) => (
                <ProductCard
                    key={product.id}
                    produs={product}
                />
            ))}
        </div>
    )
}

export default ProductGrid