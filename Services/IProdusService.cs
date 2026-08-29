using MyApi.Models;
using MyApi.Common;

namespace MyApi.Services
{
    public interface IProdusService
    {
        Task<List<Produs>> ObtineToateProduseleAsync(CancellationToken cancellationToken);
        Task<Result<Produs>> ObtineProdusAsync(int id, CancellationToken cancellationToken);
        Task<Result<Produs>> AdaugaProdusAsync(Produs produs, CancellationToken cancellationToken);
        Task<Result<Produs>> ActualizeazaProdusAsync(int id, Produs produsActualizat, CancellationToken cancellationToken);
        Task<Result<bool>> StergeProdusAsync(int id, CancellationToken cancellationToken);
    }
}
