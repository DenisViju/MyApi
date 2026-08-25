using MyApi.Models;
namespace MyApi.Services
{
    public interface IProdusService
    {
        Task<List<Produs>> ObtineToateProdusele();
        Task<Produs?> ObtineProdus(int id);
        Task<Produs> AdaugaProdus(Produs produs);
        Task<Produs?> ActualizeazaProdus(int id, Produs produsActualizat);
        Task<bool> StergeProdus(int id);
    }
}
