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
import RegisterPage from './pages/RegisterPage'
import CheckoutPage from './pages/CheckoutPage'
import AddAddressPage from './pages/AddAddressPage'
import ProfilePage from './pages/ProfilePage'
import AddressesPage from './pages/AddressesPage'
import EditAddressPage from './pages/EditAddressPage'
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
            <Route path="/register" element = {<RegisterPage />}></Route>
            <Route element={<ProtectedRoute />}>
              <Route path="/checkout" element={<CheckoutPage />} />
              <Route path="/addresses/new" element={<AddAddressPage />} />
              <Route path="/profile" element={<ProfilePage />} />
              <Route path="/addresses" element={<AddressesPage />} />
              <Route path="/addresses/:id/edit" element={<EditAddressPage />}/>
            </Route>
          </Routes>
        </CartProvider>
      </AuthProvider>
    </BrowserRouter>

  )
}

export default App