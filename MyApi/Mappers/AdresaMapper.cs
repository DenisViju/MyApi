using MyApi.Models;
using MyApi.DTOs.Adresa;

namespace MyApi.Mappers
{
    public static class AdresaMapper
    {
        public static AdresaDto ToDto(Adresa adresa)
        {
            return new AdresaDto
            {
                Id = adresa.Id,
                NumeDestinatar = adresa.NumeDestinatar,
                Strada = adresa.Strada,
                Oras = adresa.Oras,
                Judet = adresa.Judet,
                CodPostal = adresa.CodPostal,
                Tara = adresa.Tara,
                NumarTelefon = adresa.NumarTelefon,
                EstePrincipala = adresa.EstePrincipala
            };

        }

        public static Adresa ToEntity(AdresaCreateDto adresaCreateDto, int userId)
        {
            return new Adresa
            {
                UserId = userId,
                NumeDestinatar = adresaCreateDto.NumeDestinatar,
                Strada = adresaCreateDto.Strada,
                Oras = adresaCreateDto.Oras,
                Judet = adresaCreateDto.Judet,
                CodPostal = adresaCreateDto.CodPostal,
                Tara = adresaCreateDto.Tara,
                NumarTelefon = adresaCreateDto.NumarTelefon,
                EstePrincipala = adresaCreateDto.EstePrincipala
            };
        }
        public static void UpdateEntity(Adresa adresa, AdresaUpdateDto dto)
        {
            adresa.NumeDestinatar = dto.NumeDestinatar;
            adresa.Strada = dto.Strada;
            adresa.Oras = dto.Oras;
            adresa.Judet = dto.Judet;
            adresa.CodPostal = dto.CodPostal;
            adresa.Tara = dto.Tara;
            adresa.NumarTelefon = dto.NumarTelefon;
            adresa.EstePrincipala = dto.EstePrincipala;
        }


    }
}
