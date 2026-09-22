import type { Comanda } from '../types/Comanda'

const LIMITA_ANULARE_IN_MINUTE = 30

export function formateazaDataComanda(dataISO: string): string {
    return new Date(dataISO).toLocaleString('ro-RO', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
    })
}

export function comandaPoateFiAnulata(comanda: Comanda): boolean {
    const dataCrearii = new Date(comanda.dataCrearii)
    const pragAnulare = new Date(
        dataCrearii.getTime() + LIMITA_ANULARE_IN_MINUTE * 60 * 1000
    )
    const esteTimpExpirat = new Date() > pragAnulare

    return !esteTimpExpirat && comanda.status !== 'Anulata'
}