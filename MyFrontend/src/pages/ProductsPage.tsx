import { useEffect, useState } from 'react'
import { getProducts } from '../api/ProductsApi'
import type { Produs } from '../types/Produs'
import { ApiError } from '../api/ApiError'

function ProductsPage() {
  const [products, setProducts] = useState<Produs[]>([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [page, setPage] = useState(1)
  const [totalPages, setTotalPages] = useState(0)

  useEffect(() => {
    
    async function loadProducts() {
        try {
            setIsLoading(true)
            const result = await getProducts(page, 10)
            setProducts(result.data)
            setTotalPages(result.totalPages)
        }
        catch(error : unknown) {
            //momentan nu ma ajuta cu nimic ApiError
            if(error instanceof ApiError ||
               error instanceof Error) {
                setError(error.message)
            }
            else{
                setError("A aparut o eroare necunoscuta")
            }
        }
        finally {
            setIsLoading(false)
        }
    }

    loadProducts()
  }, [page])

  if (isLoading) {
    return <p>Se incarca produsele...</p>
  }
  if(error) {
    return <p>{error}</p>
  }
  return (
    <div>
      <h1>Produse</h1>

      {products.map((product) => (
        <p key={product.id}>
          {product.nume} - {product.pret} lei
        </p>
      ))}

      <div>
        <button
            onClick={() => setPage(page - 1)}
            disabled={page === 1}
        >
            Anterior
        </button>
        <span>
            Pagina {page} din {totalPages}
        </span>
        <button
            onClick={() => setPage(page + 1)}
            disabled={page === totalPages}
        >
                Urmatoarea
        </button>
      </div>
    </div>
  )
}

export default ProductsPage