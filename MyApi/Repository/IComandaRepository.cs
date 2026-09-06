using MyApi.Common;
using MyApi.DTOs.Comanda;
using MyApi.Models;

namespace MyApi.Repository
{
    public interface IComandaRepository
    {
        Task<List<Comanda>> ObtineComenzileUseruluiAsync(int userId, CancellationToken cancellationToken);
        Task<PagedResult<ComandaDto>> ObtineToateComenzileAsync
            (ComandaFiltruDto filtru, int page, int pageSize, CancellationToken cancellationToken);
        Task<Comanda?> ObtineComandaAsync(int id, int userId, CancellationToken cancellationToken);
        Task<Comanda?> ObtineOriceComandaAsync(int id, CancellationToken cancellationToken);
        Task<Comanda> CreeazaComandaAsync(Comanda comanda, CancellationToken cancellationToken);
        Task SalveazaSchimbarileAsync(CancellationToken cancellationToken);

    }
}
