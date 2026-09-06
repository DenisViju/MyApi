using MyApi.Common;
using MyApi.DTOs.Adresa;

namespace MyApi.Services
{
    public interface IAdresaService
    {
        Task<Result<List<AdresaDto>>> ObtineAdreseleUseruluiAsync(int userId, CancellationToken cancellationToken);
        Task<Result<AdresaDto>> ObtineAdresaAsync(int id, int userId, CancellationToken cancellationToken);
        Task<Result<AdresaDto>> AdaugaAdresaAsync(AdresaCreateDto adresaCreateDto, int userId, CancellationToken cancellationToken);
        Task<Result<AdresaDto>> ActualizeazaAdresaAsync
            (AdresaUpdateDto adresaUpdateDto, int id, int userId, CancellationToken cancellationToken);
        Task<Result<bool>> StergeAdresaAsync(int id, int userId, CancellationToken cancellationToken);
    }
}
