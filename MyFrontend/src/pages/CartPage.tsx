import { Link } from "react-router-dom";
import { useCart } from "../context/CartContext";


function CartPage() {

    const { items, actualizeazaCantitate, stergeProdus } = useCart()

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

                        <div>
                            <button
                                type="button"
                                onClick={() => 
                                    actualizeazaCantitate(
                                        item.produs.id, 
                                        item.cantitate - 1
                                    )
                                }
                            >
                                -
                            </button>

                            <span>Cantitate: {item.cantitate} </span>

                            <button
                                type="button"
                                onClick={() =>
                                    actualizeazaCantitate(
                                        item.produs.id,
                                        item.cantitate + 1
                                    )
                                }
                            >
                                +
                            </button>
                        </div>
                        
                        <p>
                            Subtotal: {item.cantitate * item.produs.pret} lei
                        </p>
                        <Link to={`/products/${item.produs.id}`}>Vezi detalii</Link>

                        <button
                            type="button"
                            onClick={() => 
                                stergeProdus(item.produs.id)
                            }
                        >
                            Elimina Produsul
                        </button>
                    </li>
                ))}
            </ul>

            <h2>Total: {total} lei</h2>
            <Link to="/checkout">Finalizeaza comanda</Link>
        </div>
    )
}
export default CartPage