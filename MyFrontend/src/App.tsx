import './App.css'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import { CartProvider } from './context/CartContext'
import { AuthProvider } from './context/AuthContext'

import HomePage from './pages/HomePage'
import ProductsPage from './pages/ProductsPage'
import ProductDetailsPage from './pages/ProductDetailsPage'
import Navbar from './components/Navbar'
import CartPage from './pages/CartPage'
import LoginPage from './pages/LoginPage'
import CheckoutPage from './pages/CheckoutPage'
import ProtectedRoute from './components/ProtectedRoute'

function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <CartProvider>
          <Navbar />

          <Routes>
            <Route path="/" element = {<HomePage />}></Route>
            <Route path="/products" element = {<ProductsPage />}></Route>
            <Route path="/products/:id" element = {<ProductDetailsPage />}></Route>
            <Route path="/cart" element = {<CartPage />} ></Route>
            <Route path="/login" element = {<LoginPage />}></Route>
            <Route element={<ProtectedRoute />}>
              <Route path="/checkout" element={<CheckoutPage />} />
            </Route>
          </Routes>
        </CartProvider>
      </AuthProvider>
    </BrowserRouter>

  )
}

export default App