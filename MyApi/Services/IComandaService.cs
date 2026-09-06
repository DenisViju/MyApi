using MyApi.Common;
using MyApi.DTOs.Comanda;
using MyApi.DTOs.Produs;

namespace MyApi.Services
{
    public interface IComandaService
    {
        Task<Result<ComandaDto>> CreeazaComandaAsync
            (ComandaCreateDto comandaCreateDto, int userId, CancellationToken cancellationToken);
        Task<Result<List<ComandaDto>>> ObtineComenzileUseruluiAsync(int userId, CancellationToken cancellationToken);
        Task<Result<PagedResult<ComandaDto>>> ObtineToateComenzileAsync
            (ComandaFiltruDto filtru, int page, int pageSize, CancellationToken cancellationToken);
        Task<Result<ComandaDto>> ObtineComandaAsync(int id, int userId, CancellationToken cancellationToken);
        Task<Result<ComandaDto>> ObtineOriceComandaAsync(int id, CancellationToken cancellationToken);
        Task<Result<ComandaDto>> AnuleazaComandaAsync(int id, int userId, CancellationToken cancellationToken);
        Task<Result<ComandaDto>> ActualizeazaStatusComandaAsync 
            (int id, ComandaStatusUpdateDto comandaStatusUpdateDto, CancellationToken cancellationToken);
    }
}
