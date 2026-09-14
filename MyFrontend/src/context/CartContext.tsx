import { createContext, useContext,  useState, type ReactNode } from 'react'
import type { Produs } from "../types/Produs";

export interface CartItem {
    produs: Produs
    cantitate: number
}

type CartContextType = {
    items: CartItem[]
    adaugaProdus: (produs: Produs) => void
}

export const CartContext = createContext<CartContextType | undefined>(undefined)

export function CartProvider({children}: {children: ReactNode}) {
    const [items, setItems] = useState<CartItem[]>([])

    function adaugaProdus(produs: Produs) {
        setItems((itemsCurente) => {
            const itemExistent = itemsCurente.find (
                (item) => item.produs.id === produs.id
            )

            if(itemExistent) {
                return itemsCurente.map((item) =>
                    item.produs.id == produs.id
                    ?{...item, cantitate: item.cantitate + 1}
                    :item
                )
                 
            }
            return [...itemsCurente, { produs, cantitate: 1 }]
        })

    }
     return (
    <CartContext.Provider value={{ items, adaugaProdus }}>
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