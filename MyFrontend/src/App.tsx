import './App.css'
import { BrowserRouter, Routes, Route } from 'react-router-dom'

import HomePage from './pages/HomePage'
import ProductsPage from './pages/ProductsPage'

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element = {<HomePage />}></Route>
        <Route path="/products" element = {<ProductsPage />}></Route>
      </Routes>
    </BrowserRouter>

  )
}

export default App