import './App.css'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import { CartProvider } from './context/CartContext'

import HomePage from './pages/HomePage'
import ProductsPage from './pages/ProductsPage'
import ProductDetailsPage from './pages/ProductDetailsPage'
import Navbar from './components/Navbar'

function App() {
  return (
    <BrowserRouter>
      <CartProvider>
        <Navbar />
        <Routes>
          <Route path="/" element = {<HomePage />}></Route>
          <Route path="/products" element = {<ProductsPage />}></Route>
          <Route path="/products/:id" element = {<ProductDetailsPage />}></Route>
        </Routes>
      </CartProvider>
    </BrowserRouter>

  )
}

export default App