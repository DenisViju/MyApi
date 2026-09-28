import { useEffect, useState } from 'react'
import { getProducts } from '../api/ProductsApi'
import type { Produs } from '../types/Produs'
import type { ProdusFiltru } from '../types/ProdusFiltru'
import type { Categorie } from '../types/Categorie'
import ProductGrid from '../components/ProductGrid'
import { getCategories } from '../api/CategoriesApi'
import Pagination from '../components/Pagination'
import './ProductsPage.css'


function ProductsPage() {
  const [products, setProducts] = useState<Produs[]>([])
  const [categories, setCategories] = useState<Categorie[]>([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const [page, setPage] = useState(1)
  const [totalPages, setTotalPages] = useState(0)

  const [filtru, setFiltru] = useState<ProdusFiltru>({})
  const [inputNume, setInputNume] = useState('')

  const [categorieId, setCategorieId] = useState('')
  const [pretMinim, setPretMinim] = useState('')
  const [pretMaxim, setPretMaxim] = useState('')

  const [sortBy, setSortBy] = useState('')
  const [descending, setDescending] = useState(false)
  
  const [showFilters, setShowFilters] = useState(false)

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

  useEffect(() => {
    async function loadCategories() {
        try {
            const result = await getCategories(1, 100, {})
            setCategories(result.data)
        } catch (error) {
            console.error(error)
        }
    }
    loadCategories()
  }, [])

function cautaProduse() {
  const nume = inputNume.trim()

  setFiltru(prev => ({
    ...prev,
    nume: nume === '' ? undefined : nume
  }))

  setPage(1)
}

  function aplicaFiltre() {
    setFiltru(prev => ({
        ...prev,

        categorieId:
            categorieId === ''
                ? undefined
                : Number(categorieId),

        pretMinim:
            pretMinim === ''
                ? undefined
                : Number(pretMinim),

        pretMaxim:
            pretMaxim === ''
                ? undefined
                : Number(pretMaxim),

        sortBy:
            sortBy === ''
                ? undefined
                : sortBy,

        descending:
            sortBy === ''
                ? undefined
                : descending

    }))
    
    setPage(1)
    setShowFilters(false)

  }

function reseteazaFiltre() {
    setCategorieId('')
    setPretMinim('')
    setPretMaxim('')

    setSortBy('')
    setDescending(false)

    setFiltru(prev => ({
        ...prev,
        categorieId: undefined,
        pretMinim: undefined,
        pretMaxim: undefined,
        sortBy: undefined,
        descending: undefined
    }))

    setPage(1)
    setShowFilters(false)
}

  if (isLoading) {
    return <p>Se incarca produsele...</p>
  }
  if(error) {
    return <p>{error}</p>
  }
  return (
    <div className="products-page">
        <header className='products-header'>
            <h1>Produse</h1>

            <div className='product-search'>
                <input
                    type="text"
                    value={inputNume}
                    onChange={(event) => setInputNume(event.target.value)}
                    onKeyDown={(event) => {
                        if (event.key === 'Enter') {
                            cautaProduse()
                        }
                    }}
                    placeholder="Cauta produse..."
                />

                <button
                    type="button"
                    onClick={cautaProduse}
                >
                    Cauta
                </button>
            </div>
        </header>
      

        <div className="filters-section">

        <button
            type="button"
            className="filters-toggle"
            onClick={() => setShowFilters(prev => !prev)}
        >
            {showFilters ? 'Ascunde filtrele' : 'Filtre'}
        </button>

        {showFilters && (
            <div className="filters-panel">

                <h2>Filtre</h2>

                <div className="filter-group">
                    <label htmlFor="categorie">
                        Categorie
                    </label>

                    <select
                        id="categorie"
                        value={categorieId}
                        onChange={(event) =>
                            setCategorieId(event.target.value)
                        }
                    >
                        <option value="">
                            Toate categoriile
                        </option>

                        {categories.map(categorie => (
                            <option
                                key={categorie.id}
                                value={categorie.id}
                            >
                                {categorie.nume}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="filter-group">
                    <label htmlFor="pret-minim">
                        Preț minim
                    </label>

                    <input
                        id="pret-minim"
                        type="number"
                        min="0"
                        step="0.01"
                        value={pretMinim}
                        onChange={(event) =>
                            setPretMinim(event.target.value)
                        }
                        placeholder="Pret minim"
                    />
                </div>

                <div className="filter-group">
                    <label htmlFor="pret-maxim">
                        Preț maxim
                    </label>

                    <input
                        id="pret-maxim"
                        type="number"
                        min="0"
                        step="0.01"
                        value={pretMaxim}
                        onChange={(event) =>
                            setPretMaxim(event.target.value)
                        }
                        placeholder="Pret maxim"
                    />
                </div>

                <div className="filter-group">
                    <label htmlFor="sortare">
                        Sorteaza dupa
                    </label>

                    <select
                        id="sortare"
                        value={sortBy}
                        onChange={(event) =>
                            setSortBy(event.target.value)
                        }
                    >
                        <option value="">
                            Implicit
                        </option>

                        <option value="pret">
                            Pret
                        </option>

                        <option value="nume">
                            Nume
                        </option>
                    </select>
                </div>

                <div className="filter-group">
                    <label htmlFor="ordine">
                        Ordine
                    </label>

                    <select
                        id="ordine"
                        value={descending ? 'desc' : 'asc'}
                        onChange={(event) =>
                            setDescending(event.target.value === 'desc')
                        }
                    >
                        <option value="asc">
                            Crescator
                        </option>

                        <option value="desc">
                            Descrescator
                        </option>
                    </select>
                </div>

                <div className="filters-actions">

                    <button
                        type="button"
                        onClick={reseteazaFiltre}
                    >
                        Reseteaza filtrele
                    </button>

                    <button
                        type="button"
                        onClick={aplicaFiltre}
                    >
                        Aplica filtre
                    </button>

                </div>

            </div>
        )}
    </div>

   <div className="products-results">
        {products.length === 0 ? (
            <div className="products-empty">
            <h2>Niciun produs gasit</h2>
            </div>
        ) : (
            <>
            <div className="products-toolbar">

                <div className="products-results-header">
                    <span>
                        {products.length}{' '}
                        {products.length === 1 ? 'produs' : 'produse'}
                    </span>
                </div>

                {totalPages > 1 && (
                    <Pagination
                        page={page}
                        totalPages={totalPages}
                        onPageChange={setPage}
                    />
                )}

            </div>

            <ProductGrid products={products} />

            {totalPages > 1 && (
                <Pagination
                page={page}
                totalPages={totalPages}
                onPageChange={setPage}
                />
            )}
            </>
        )}
    </div>

    </div>
  )
}

export default ProductsPage