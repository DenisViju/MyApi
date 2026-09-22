import { useEffect, useState } from "react";
import { useAuth } from "../context/AuthContext";
import type { Comanda } from "../types/Comanda";
import { useParams, Link } from "react-router-dom";
import { getOrder } from "../api/OrdersApi";
import { formateazaDataComanda } from "../utils/ComandaUtils";
import CancelOrderButton from "../components/CancelOrderButton";

function OrderDetailsPage() {
    const {accessToken} = useAuth()
    
    const { id } = useParams<{id: string}>()
    const [order, setOrder] = useState<Comanda | null>(null)
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        async function loadOrder() {
            if(!accessToken) {
                setError('Nu esti autentificat')
                setIsLoading(false)
                return
            }

            if(!id) {
                setError('ID-ul comenzii lipseste')
                setIsLoading(false)
                return
            }

            const idComanda = Number(id)

            if(Number.isNaN(idComanda)) {
                setError('ID-ul comenzii este invalid')
                setIsLoading(false)
                return
            } 

            try {
                setIsLoading(true)
                setError(null)

                const comandaIncarcata = await getOrder(idComanda, accessToken)
                setOrder(comandaIncarcata)
            } catch(error: unknown) {
                if(error instanceof Error) {
                    setError(error.message)
                } else {
                    setError('A aparut o eroare necunoscuta')
                }
            } finally {
                setIsLoading(false)
            }
        }

        loadOrder()
    } ,[accessToken, id])


    if(isLoading) {
        return(
            <p>Comanda se incarca..</p>
        )
    }

    if(!order) {
        return <p>Comanda nu a fost gasita</p>
    }

    return( 
        <div>
            <h1>Detalii comanda</h1>
            
            <div>
                <h2>Comanda {order.status}</h2>
                <p>Numar comanda: #{order.id}</p>
                <p>Data: {formateazaDataComanda(order.dataCrearii)}</p>
                <p>Total comanda: {order.total} lei</p>
            </div>
                        
            <div>
                <h2>Produse comandate:</h2>
                <ul>
                    {order.elementeComanda.map((elementComanda) => (
                        <li key={elementComanda.id}>
                            <p>Produs: {elementComanda.numeProdus}</p>
                            <p>Cantitate: {elementComanda.cantitate}</p>
                            <p>Preț unitar: {elementComanda.pretUnitar} lei</p>
                        </li>
                    ))}
                </ul>
            </div>

            <div>
                <h2>Adresa de livrare</h2>
                <p><strong>Destinatar:</strong> {order.numeDestinatar}</p>
                <p><strong>Telefon:</strong> {order.telefonDestinatar}</p>
                <p><strong>Adresa:</strong> {order.strada}, {order.oras}, {order.judet}, {order.codPostal}, {order.tara}</p>
            </div>
           
            <div>
                <CancelOrderButton
                order={order}
                onCancelled={(comandaAnulata) => setOrder(comandaAnulata)}
                />
            </div>
            
            
            
            <Link to="/orders">Inapoi la comenzi</Link>
            {error && <p>{error}</p>}
        </div>

    )
}

export default OrderDetailsPage