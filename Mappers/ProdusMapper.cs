using MyApi.DTOs.Produs;
using MyApi.Models;

namespace MyApi.Mappers
{
    public static class ProdusMapper
    {
        public static ProdusDto ToDto(Produs produs)
        {
            return new ProdusDto
            {
                Id = produs.Id,
                Nume = produs.Nume,
                Pret = produs.Pret,
                CategorieId = produs.CategorieId,
                NumeCategorie = produs.Categorie?.Nume,
                RowVersion = produs.RowVersion
            };
        }

        public static Produs ToEntity(ProdusCreateDto produsCreateDto)
        {
            return new Produs
            {
                Nume = produsCreateDto.Nume,
                Pret = produsCreateDto.Pret,
                CategorieId = produsCreateDto.CategorieId

            };
        }
        public static Produs ToEntity(ProdusUpdateDto produsUpdateDto)
        {
            return new Produs
            {
                Nume = produsUpdateDto.Nume,
                Pret = produsUpdateDto.Pret,
                CategorieId = produsUpdateDto.CategorieId,
                RowVersion = produsUpdateDto.RowVersion

            };
        }

    }
}
