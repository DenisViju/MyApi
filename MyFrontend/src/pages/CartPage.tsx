import { Link } from "react-router-dom";
import { useCart } from "../context/CartContext";


function CartPage() {

    const { items } = useCart()

    const total = items.reduce(
        (suma, item) => suma + item.produs.pret * item.cantitate,
        0
    )

    if(items.length === 0) {
        return (
            <div>
                <h1>Cosul tau</h1>
                <p>Cosul tau este gol</p>
                <Link to="/products">Vezi produsele</Link>
            </div>
        )
    }

    return(
        <div>
            <h1>Cosul tau</h1>

            <ul>
                {items.map((item) => (
                    <li key={item.produs.id}>
                        <h2>{item.produs.nume}</h2>
                        <p>
                            Cantitate: {item.cantitate} x {item.produs.pret} lei
                        </p>
                        <p>
                            Subtotal: {item.cantitate * item.produs.pret} lei
                        </p>
                        <Link to={`/products/${item.produs.id}`}>Vezi detalii</Link>
                    </li>
                ))}
            </ul>

            <h2>Total: {total} lei</h2>
        </div>
    )
}
export default CartPage