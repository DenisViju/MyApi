import { getOrders } from "../api/OrdersApi";
import { useAuth } from "../context/AuthContext";
import { useState, useEffect } from "react";
import type { Comanda } from "../types/Comanda";
import OrderDetails from "../components/OrderDetails";

function OrdersPage() {
    const {accessToken} = useAuth()
    const [orders, setOrders] = useState<Comanda[]>([])
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        async function loadOrders() {
            if(!accessToken) {
                setError('Nu esti autentificat')
                setIsLoading(false)
                return
            }

            try {
                setIsLoading(true)
                setError(null)
                const comenziIncarcate = await getOrders(accessToken)
                setOrders(comenziIncarcate)
            } catch(error: unknown){
                if(error instanceof Error) {
                    setError(error.message)
                } else {
                    setError('A aparut o eroare necunoscuta')
                }
            } finally {
                setIsLoading(false)
            }
        } 
        loadOrders()
    }, [accessToken])

    if(isLoading)  {
        return(
            <p>Se incarca comenzile..</p>
        )
    }

    return(
        <div>
            <h1>Comenzile mele</h1>

            {orders.length === 0 ? (
                <p>Nu ai comenzi</p>
            ) : (
                orders.map((order) => (
                    <OrderDetails 
                        key={order.id}
                        order={order}
                    />
                ))
            )}

            {error && <p>{error}</p>}
        </div>
    )
}

export default OrdersPage