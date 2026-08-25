using MyApi.Models;
namespace MyApi.Services
{
    public interface IProdusService
    {
        Task<List<Produs>> ObtineToateProdusele();
        Produs? ObtineProdus(int id);
        Produs AdaugaProdus(Produs produs);
        Produs? ActualizeazaProdus(int id, Produs produsActualizat);
        bool StergeProdus(int id);
    }
}
