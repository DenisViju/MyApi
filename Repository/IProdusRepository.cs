using MyApi.Models;
using MyApi.Common;
using MyApi.DTOs.Produs;

namespace MyApi.Repository
{
    public interface IProdusRepository
    {
        Task<PagedResult<ProdusDto>> ObtineToateProduseleAsync
            (ProdusFiltruDto filtru, int page, int pageSize, CancellationToken cancellationToken);
        Task<Produs?> ObtineProdusAsync(int id, CancellationToken cancellationToken);
        Task<Produs> AdaugaProdusAsync(Produs produs, CancellationToken cancellationToken);
        Task<Produs?> ActualizeazaProdusAsync(int id, Produs produsActualizat, CancellationToken cancellationToken);
        Task<bool> StergeProdusAsync(int id, CancellationToken cancellationToken);
        Task<Categorie?> GasesteCategorieAsync(int id, CancellationToken cancellationToken);
        Task<bool> ExistaProdusCuNumeleAsync(string nume, CancellationToken cancellationToken);
        Task<bool> ExistaAltProdusCuNumeleAsync(string nume, int id, CancellationToken cancellationToken);
    }
}
