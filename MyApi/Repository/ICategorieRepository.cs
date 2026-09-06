using MyApi.Common;
using MyApi.DTOs.Categorie;
using MyApi.Models;

namespace MyApi.Repository
{
    public interface ICategorieRepository
    {
        Task<PagedResult<CategorieDto>> ObtineCategoriiAsync
            (CategorieFiltruDto filtru, int page, int pageSize, CancellationToken cancellationToken);
        Task<Categorie?> ObtineCategorieAsync (int id, CancellationToken cancellationToken);
        Task<Categorie> CreeazaCategorieAsync(Categorie categorieNoua, CancellationToken cancellationToken);
        Task<Categorie?> ActualizeazaCategorieAsync
            (int id, Categorie categorieActualizata, CancellationToken cancellationToken);
        Task StergeCategorieAsync(Categorie categorie, CancellationToken cancellationToken);
        Task<bool> ExistaCategorieCuNumeleAsync(string nume, CancellationToken cancellationToken);
        Task<bool> ExistaAltaCategorieCuNumeleAsync(string nume, int id, CancellationToken cancellationToken);
        Task<Categorie?> ObtineCategorieCuProduseAsync(int id, CancellationToken cancellationToken);
    }
}
