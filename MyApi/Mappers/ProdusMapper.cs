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
                Descriere = produs.Descriere,
                Pret = produs.Pret,
                Stoc = produs.Stoc,
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
                Descriere = produsCreateDto.Descriere,
                Pret = produsCreateDto.Pret,
                Stoc = produsCreateDto.Stoc,
                CategorieId = produsCreateDto.CategorieId

            };
        }
        public static Produs ToEntity(ProdusUpdateDto produsUpdateDto)
        {
            return new Produs
            {
                Nume = produsUpdateDto.Nume,
                Descriere = produsUpdateDto.Descriere,
                Pret = produsUpdateDto.Pret,
                Stoc = produsUpdateDto.Stoc,
                CategorieId = produsUpdateDto.CategorieId,
                RowVersion = produsUpdateDto.RowVersion

            };
        }

    }
}
