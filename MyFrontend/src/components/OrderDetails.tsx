import type { Comanda } from "../types/Comanda"
import { Link } from "react-router-dom"
import { formateazaDataComanda } from "../utils/ComandaUtils"
import CancelOrderButton from "./CancelOrderButton"

type OrderDetailsProps = {
    order: Comanda
    onCancelled: (comandaAnulata: Comanda) => void
}

function OrderDetails({ order, onCancelled }: OrderDetailsProps) {
    
    return (
        <div>
            <h2>Comanda #{order.id}</h2>
            <h3>{order.status}</h3>
            <p>{formateazaDataComanda(order.dataCrearii)}</p>
            <p>{order.total} lei</p>

            <CancelOrderButton
                order={order}
                onCancelled={onCancelled}
            />

            <Link to={`/orders/${order.id}`}>
                Detalii comanda
            </Link>
        </div>
    )
}

export default OrderDetails
