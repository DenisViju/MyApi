import { useState } from "react"

type NavbarProps = {
  titlu: string
  afiseazaCos: boolean
}

function Navbar(props : NavbarProps) {
    const [numarProduse, setNumarProduse] = useState(0)
    const [nume, setNume] = useState("")

    function adaugaProdus() {
        setNumarProduse(numarProduseActual => numarProduseActual + 1)
    }
    function stergeProdus() {
        setNumarProduse(numarProduseActual => 
            numarProduseActual > 0 
                ? numarProduseActual - 1
                : 0
        )
    }
    function actualizeazaNume(eveniment: React.ChangeEvent<HTMLInputElement>) {
        setNume(eveniment.target.value)
    }
    return (
        <><header>
            <h1>{props.titlu}</h1>
            {props.afiseazaCos &&
                <span>Cos ({numarProduse}) </span>}
            <button onClick={adaugaProdus}>
                Adauga Produs
            </button>
            <span>   </span>
            <button onClick={stergeProdus}>
                Sterge Produs
            </button>
        </header><label>
                Nume:
                <input onChange={actualizeazaNume} />
                <p>Salut, {nume} </p>
            </label></>
    )
}

export default Navbar

