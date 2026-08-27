using MyApi.Models;

namespace MyApi.Repository
{
    public interface IProdusRepository
    {
        Task<List<Produs>> ObtineToateProduseleAsync();
        Task<Produs?> ObtineProdusAsync(int id);
        Task<Produs?> AdaugaProdusAsync(Produs produs);
        Task<Produs?> ActualizeazaProdusAsync(int id, Produs produsActualizat);
        Task<bool> StergeProdusAsync(int id);
        Task<Categorie?> GasesteCategorieAsync(int id);
    }
}
