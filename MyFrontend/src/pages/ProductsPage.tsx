import { useEffect, useState } from 'react'
import { getProducts } from '../api/ProductsApi'
import type { Produs } from '../types/Produs'
import type { ProdusFiltru } from '../types/ProdusFiltru'
import ProductCard from '../components/ProductCard'


function ProductsPage() {
  const [products, setProducts] = useState<Produs[]>([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [page, setPage] = useState(1)
  const [totalPages, setTotalPages] = useState(0)
  const [filtru, setFiltru] = useState<ProdusFiltru>({})
  const [inputNume, setInputNume] = useState('')

  useEffect(() => {
    
    async function loadProducts() {
        try {
            setIsLoading(true)
            setError(null)
            const result = await getProducts(page, 10, filtru)
            setProducts(result.data)
            setTotalPages(result.totalPages)
        }
        catch(error : unknown) {
            if(error instanceof Error) {
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
  }, [page, filtru])

    function cautaProduse() {
    setFiltru({
        ...filtru,
        nume: inputNume
    })
    setPage(1)
  }

  if (isLoading) {
    return <p>Se incarca produsele...</p>
  }
  if(error) {
    return <p>{error}</p>
  }
  return (
    <div>
      <h1>Produse</h1>
      
       <input
        type="text"
        value={inputNume}
        onChange={(event) => setInputNume(event.target.value)}
        onKeyDown={(event) => {
            if (event.key === 'Enter') {
                cautaProduse()
            }
        }}
        placeholder="Cauta dupa nume"
       />
      <button onClick={cautaProduse}>
        Cauta
      </button>

    {products.length === 0 ? (
        <p>Niciun produs gasit.</p>
    ) : (
        products.map((product) => (
            <ProductCard
                key={product.id}
                produs={product}
            />
        ))
    )}
    {totalPages > 0 && (
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
    )}
    </div>
  )
}

export default ProductsPage