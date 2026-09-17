export interface ElementComandaCreate {
  produsId: number
  cantitate: number
}

export interface ComandaCreateRequest {
  adresaId: number
  elementeComandaCreateDto: ElementComandaCreate[]
}

export interface ElementComanda {
  id: number
  produsId: number
  numeProdus: string
  cantitate: number
  pretUnitar: number
}

export interface Comanda {
  id: number
  userId: number

  strada: string
  oras: string
  judet: string
  tara: string
  codPostal: string
  numeDestinatar: string
  telefonDestinatar: string

  dataCrearii: string
  status: string
  total: number

  elementeComanda: ElementComanda[]
  rowVersion: string
}