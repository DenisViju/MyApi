using MyApi.Models;
using MyApi.Common;

namespace MyApi.Services
{
    public interface IProdusService
    {
        Task<List<Produs>> ObtineToateProduseleAsync();
        Task<Result<Produs>> ObtineProdusAsync(int id);
        Task<Result<Produs>> AdaugaProdusAsync(Produs produs);
        Task<Result<Produs>> ActualizeazaProdusAsync(int id, Produs produsActualizat);
        Task<Result<bool>> StergeProdusAsync(int id);
    }
}
