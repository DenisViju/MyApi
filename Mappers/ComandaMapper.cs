using MyApi.DTOs.Comanda;
using MyApi.Models;

namespace MyApi.Mappers
{
    public static class ComandaMapper
    {
        public static ComandaDto ToDto(Comanda comanda, List<ElementComandaDto> elementeComandaDto)
        {
    
            return new ComandaDto
            {
                Id = comanda.Id,
                UserId = comanda.UserId,    
                Strada = comanda.Strada,
                Oras = comanda.Oras,
                Judet = comanda.Judet,
                Tara = comanda.Tara,    
                CodPostal = comanda.CodPostal,
                NumeDestinatar = comanda.NumeDestinatar,
                TelefonDestinatar = comanda.TelefonDestinatar,
                DataCrearii = comanda.DataCrearii,
                Status = comanda.Status,
                Total = comanda.Total,
                ElementeComanda = elementeComandaDto,
                RowVersion = comanda.RowVersion

            };


        }
        public static Comanda ToEntity
            (Adresa adresa, List<ElementComanda> elementeComanda, int userId, DateTime dataCrearii, decimal total)
        {
                return new Comanda
            {
                    UserId = userId,
                    Strada = adresa.Strada,
                    Oras = adresa.Oras,
                    Judet = adresa.Judet,
                    Tara = adresa.Tara,
                    CodPostal = adresa.CodPostal,   
                    NumeDestinatar = adresa.NumeDestinatar,
                    TelefonDestinatar = adresa.NumarTelefon,
                    ElementeComanda = elementeComanda,
                    DataCrearii = dataCrearii,
                    Total = total
               
            };
        }
    }
       

}
