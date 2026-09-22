import type { Comanda } from "../types/Comanda"
import { Link } from "react-router-dom"

type OrderDetailsProps = {
    order: Comanda
}

function OrderDetails({order} : OrderDetailsProps) {
    const dataFormatata = new Date(order.dataCrearii).toLocaleString("ro-RO", {
        day: "2-digit",
        month: "2-digit",
        year: "numeric",
        hour: "2-digit",
        minute: "2-digit",
    });
    return(
        <div>
            <h2>Comanda #{order.id}</h2>
            <h3>{order.status}</h3>
            <p>{dataFormatata}</p>
            <p>{order.total} lei</p>
            <Link to={`/orders/${order.id}`}>Detalii comanda</Link>
        </div>
    )
}

export default OrderDetails


                
