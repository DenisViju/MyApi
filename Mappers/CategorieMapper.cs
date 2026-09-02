using MyApi.DTOs.Categorie;
using MyApi.Models;

namespace MyApi.Mappers
{
    public static class CategorieMapper
    {
        public static CategorieDto ToDto(Categorie categorie)
        {
            return new CategorieDto
            {
                Id = categorie.Id,
                Nume = categorie.Nume,
                RowVersion = categorie.RowVersion
            };
        }
        public static Categorie ToEntity(CategorieCreateDto  categorieCreateDto)
        {
            return new Categorie
            {
                Nume = categorieCreateDto.Nume
            };
        }
        public static Categorie ToEntity(CategorieUpdateDto categorieUpdateDto)
        {
            return new Categorie
            {
                Nume = categorieUpdateDto.Nume,
                RowVersion = categorieUpdateDto.RowVersion
            };
        }
    }
}
