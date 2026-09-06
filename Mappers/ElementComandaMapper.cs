using MyApi.DTOs.Comanda;
using MyApi.Models;

namespace MyApi.Mappers
{
    public static class ElementComandaMapper
    {
        public static ElementComandaDto ToDto(ElementComanda elementComanda)
        {
            return new ElementComandaDto
            {
                Id = elementComanda.Id,
                ProdusId = elementComanda.ProdusId,
                NumeProdus = elementComanda.Produs.Nume,
                Cantitate = elementComanda.Cantitate,
                PretUnitar = elementComanda.PretUnitar
            };
        }

        public static ElementComanda ToEntity(ElementComandaCreateDto elementComandaCreateDto, Produs produs)
        {

            return new ElementComanda
            {
                ProdusId = elementComandaCreateDto.ProdusId,
                Produs = produs,
                Cantitate = elementComandaCreateDto.Cantitate,
                PretUnitar = produs.Pret

            };
        }
    }
}
