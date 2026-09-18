export interface Adresa {
    id: number
    numeDestinatar: string
    strada: string
    oras: string
    judet: string
    codPostal: string
    tara: string
    numarTelefon: string
    estePrincipala: boolean
}

export interface AdresaCreateRequest {
    numeDestinatar: string
    strada: string
    oras: string
    judet: string
    codPostal: string
    tara: string
    numarTelefon: string
    estePrincipala: boolean
}