using MyApi.Models;
using MyApi.Repository;
namespace MyApi.Services
{
    public class ProdusService : IProdusService
    {
        private readonly IProdusRepository repository;
        public ProdusService(IProdusRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<Produs>> ObtineToateProduseleAsync()
        {
           return await repository.ObtineToateProduseleAsync();
        }
        public async Task<Produs?> ObtineProdusAsync(int id)
        {
            return await repository.ObtineProdusAsync(id);
                
        }

        public async Task<Produs> AdaugaProdusAsync(Produs produs)
        {
            return await repository.AdaugaProdusAsync(produs);
        }

        public async Task<Produs?> ActualizeazaProdusAsync(int id, Produs produsActualizat) 
        {
            return await repository.ActualizeazaProdusAsync(id, produsActualizat);
        }

        public async Task<bool> StergeProdusAsync(int id)
        {
            return await repository.StergeProdusAsync(id);

        }
        public async Task<Categorie?> GasesteCategorieAsync(int id)
        {
           return await repository.GasesteCategorieAsync(id);

        }
    }
}
