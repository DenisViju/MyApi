using MyApi.Common;
using MyApi.DTOs;
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
        public async Task<Result<Produs>> ObtineProdusAsync(int id)
        {
            var produs = await repository.ObtineProdusAsync(id);

            if (produs == null)
            {
                return Result<Produs>.Fail(
                    "Produsul nu exista",
                    ResultErrorType.NotFound);
            }
                
            return Result<Produs>.Ok(produs);
                
        }

        public async Task<Result<Produs>> AdaugaProdusAsync(Produs produs)
        {
            Categorie? categorie = await repository.GasesteCategorieAsync(produs.CategorieId);
            if (categorie == null)
            {
                return Result<Produs>.Fail(
                    "Nu exista aceasta categorie pentru produs",
                    ResultErrorType.NotFound);
            }
                
            bool exista = await repository.ExistaProdusCuNumeleAsync(produs.Nume!);
            if (exista)
            {
                return Result<Produs>.Fail(
                    "Exista deja un produs cu acest nume.",
                    ResultErrorType.Conflict);
            }

            Produs produsSalvat = await repository.AdaugaProdusAsync(produs);

            return Result<Produs>.Ok(produsSalvat);
        }

        public async Task<Result<Produs>> ActualizeazaProdusAsync(int id, Produs produsActualizat) 
        {
             

            Produs? produs = await repository.ObtineProdusAsync(id);
            if (produs == null)
            {
                return Result<Produs>.Fail(
                    "Produsul nu exista",
                    ResultErrorType.NotFound);
            }

            Categorie? categorie =
                await repository.GasesteCategorieAsync(produsActualizat.CategorieId);

            if (categorie == null)
            {
                return Result<Produs>.Fail(
                   "Nu exista aceasta categorie pentru produs",
                   ResultErrorType.NotFound);
            }

            bool exista = await repository.ExistaAltProdusCuNumeleAsync(produsActualizat.Nume!, id);
            if (exista)
            {
                return Result<Produs>.Fail(
                    "Exista deja un produs cu acest nume",
                    ResultErrorType.Conflict);
                   
            }


            Produs? produsSalvat = await repository.ActualizeazaProdusAsync(id, produsActualizat);
            if (produsSalvat == null)
            {
                return Result<Produs>.Fail(
                    "Produsul nu a putut fi actualizat.",
                    ResultErrorType.NotFound);
            }


            return Result<Produs>.Ok(produsSalvat);
        }

        public async Task<Result<bool>> StergeProdusAsync(int id)
        {
            Produs? produs = await repository.ObtineProdusAsync(id);
            if (produs == null)
            {
                return Result<bool>.Fail(
                    "Produsul nu exista",
                    ResultErrorType.NotFound);
            }
            bool sters = await repository.StergeProdusAsync(id);
            
            return Result<bool>.Ok(sters);  

        }
        
    }
}
