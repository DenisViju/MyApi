import { Link } from 'react-router-dom'
import { useCart } from '../context/CartContext'
import { useAuth } from '../context/AuthContext'

function Navbar() {
  const { items } = useCart()
  const { user, delogheaza } = useAuth()

  const numarProduseInCos = items.reduce(
    (total, item) => total + item.cantitate,
    0
  )

  return (
    <header>
      <Link to="/">Magazin Online</Link>

      <nav aria-label="Navigare principala">
        <Link to="/">Acasa</Link>
        <Link to="/products">Produse</Link>
      </nav>

        {user ?  (
            <>
                <span>Salut, {user.username}</span>

                <button type="button" onClick={delogheaza}> 
                    Deconectare
                </button>
            </>
        ) : (
            <Link to="/login">Autentificare</Link>
        )}
      <Link to="/cart">Cos ({numarProduseInCos})</Link>
    </header>
  )
}

export default Navbar