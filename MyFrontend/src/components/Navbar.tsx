import { Link } from 'react-router-dom'
import { useCart } from '../context/CartContext'

function Navbar() {
  const { items } = useCart()

  const numarProduseInCos = items.reduce(
    (total, item) => total + item.cantitate,
    0
  )

  return (
    <header>
      <Link to="/">Magazin Online</Link>

      <nav aria-label="Navigare principală">
        <Link to="/">Acasă</Link>
        <Link to="/products">Produse</Link>
      </nav>

      <span>Coș ({numarProduseInCos})</span>
    </header>
  )
}

export default Navbar