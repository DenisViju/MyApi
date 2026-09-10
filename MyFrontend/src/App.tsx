import './App.css'
import Navbar from './components/Navbar'

function App() {
  return (
    <div>
      <Navbar
       titlu="Magazin Online"
       afiseazaCos={true}
      />
  
      <main>
        <h2>Bine ati venit la magazinul nostru online</h2>
        <p>Descopera produsele noastre</p>
        
        <button>Vezi produsele</button>
      </main>

    </div>
  )
}

export default App