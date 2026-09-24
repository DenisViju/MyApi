import { createContext, useContext, useEffect, useState, type ReactNode } from 'react'
import type { Produs } from "../types/Produs";

export interface CartItem {
    produs: Produs
    cantitate: number
}

type CartContextType = {
    items: CartItem[]
    adaugaProdus: (produs: Produs, cantitate: number) => void
    actualizeazaCantitate: (produsId: number, cantitateNoua: number) => void
    stergeProdus: (produsId: number) => void
    golesteCos: () => void
}

export const CartContext = createContext<CartContextType | undefined>(undefined)

export function CartProvider({children}: {children: ReactNode}) {
    const [items, setItems] = useState<CartItem[]>(() => {
        const cartSalvat = localStorage.getItem('cart')
        if(!cartSalvat) {
            return []
        }

        try {
            return JSON.parse(cartSalvat) as CartItem[]
        } catch {
            return []
        }
    })

    useEffect(() => {
        localStorage.setItem('cart', JSON.stringify(items))
    }, [items])

    function adaugaProdus(produs: Produs, cantitate: number) {
        setItems((itemsCurente) => {
            const itemExistent = itemsCurente.find(
                (item) => item.produs.id === produs.id
            )

            if (itemExistent) {
                return itemsCurente.map((item) =>
                    item.produs.id === produs.id
                        ? {
                            ...item,
                            cantitate: Math.min(
                                item.cantitate + cantitate,
                                produs.stoc
                            )
                        }
                        : item
                )
            }

            return [
                ...itemsCurente,
                {
                    produs,
                    cantitate: Math.min(cantitate, produs.stoc)
                }
            ]
        })
    }
    
    function actualizeazaCantitate(produsId: number, cantitateNoua: number) {
        setItems((itemsCurente) => {
            const itemExistent = itemsCurente.find(
                (item) => item.produs.id === produsId
            )

            if(!itemExistent){
                return itemsCurente
            }

            if(cantitateNoua <= 0) {
                return itemsCurente.filter(
                    (item) => item.produs.id !== produsId
                )
            }

            const cantitateValida = Math.min(
                cantitateNoua,
                itemExistent.produs.stoc
            )

            return itemsCurente.map((item) => 
                item.produs.id === produsId 
                    ? {...item, cantitate: cantitateValida}
                    : item
            )
        })
    }

    function stergeProdus(produsId: number) {
        setItems((itemsCurente) => 
            itemsCurente.filter((item) => 
                item.produs.id !== produsId
        ))
    }

    function golesteCos() {
        setItems([])
    }

     return (
    <CartContext.Provider value={{ items, adaugaProdus, actualizeazaCantitate, stergeProdus, golesteCos }}>
      {children}
    </CartContext.Provider>
  )
}

export function useCart() {
  const cart = useContext(CartContext)

  if (!cart) {
    throw new Error('useCart trebuie folosit in interiorul CartProvider')
  }

  return cart
}