using MyApi.Common;
using MyApi.DTOs.Categorie;


namespace MyApi.Services
{
    public interface ICategorieService
    {
        Task<Result<PagedResult<CategorieDto>>> ObtineCategoriiAsync
           (CategorieFiltruDto filtru, int page, int pageSize, CancellationToken cancellationToken);
        Task<Result<CategorieDto>> ObtineCategorieAsync (int id, CancellationToken cancellationToken);
        Task<Result<CategorieDto>> AdaugaCategorieAsync
            (CategorieCreateDto categorieCreateDto, CancellationToken cancellationToken);
        Task<Result<CategorieDto>> ActualizeazaCategorieAsync
            (int id, CategorieUpdateDto categorieUpdateDto, CancellationToken cancellationToken);
        Task<Result<bool>> StergeCategorieAsync(int id, CancellationToken cancellationToken);
    }
}
