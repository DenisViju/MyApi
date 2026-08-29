using MyApi.Common;
using MyApi.Models;
using MyApi.Repository;
using MyApi.DTOs;

namespace MyApi.Services
{
    public class ProdusService : IProdusService
    {
        private readonly IProdusRepository repository;
        public ProdusService(IProdusRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<Produs>> ObtineToateProduseleAsync(ProdusFiltruDto filtru, CancellationToken cancellationToken)
        {
           return await repository.ObtineToateProduseleAsync(filtru, cancellationToken);
        }
        public async Task<Result<Produs>> ObtineProdusAsync(int id, CancellationToken cancellationToken)
        {
            var produs = await repository.ObtineProdusAsync(id, cancellationToken);

            if (produs == null)
            {
                return Result<Produs>.Fail(
                    "Produsul nu exista",
                    ResultErrorType.NotFound);
            }
                
            return Result<Produs>.Ok(produs);
                
        }

        public async Task<Result<Produs>> AdaugaProdusAsync(Produs produs, CancellationToken cancellationToken)
        {
            Categorie? categorie = await repository.GasesteCategorieAsync(produs.CategorieId, cancellationToken);
            if (categorie == null)
            {
                return Result<Produs>.Fail(
                    "Nu exista aceasta categorie pentru produs",
                    ResultErrorType.NotFound);
            }
                
            bool exista = await repository.ExistaProdusCuNumeleAsync(produs.Nume!, cancellationToken);
            if (exista)
            {
                return Result<Produs>.Fail(
                    "Exista deja un produs cu acest nume.",
                    ResultErrorType.Conflict);
            }

            Produs produsSalvat = await repository.AdaugaProdusAsync(produs, cancellationToken);

            return Result<Produs>.Ok(produsSalvat);
        }

        public async Task<Result<Produs>> ActualizeazaProdusAsync
            (int id, Produs produsActualizat, CancellationToken cancellationToken) 
        {
             

            Produs? produs = await repository.ObtineProdusAsync(id, cancellationToken);
            if (produs == null)
            {
                return Result<Produs>.Fail(
                    "Produsul nu exista",
                    ResultErrorType.NotFound);
            }

            Categorie? categorie =
                await repository.GasesteCategorieAsync(produsActualizat.CategorieId, cancellationToken);

            if (categorie == null)
            {
                return Result<Produs>.Fail(
                   "Nu exista aceasta categorie pentru produs",
                   ResultErrorType.NotFound);
            }

            bool exista = await repository.ExistaAltProdusCuNumeleAsync(produsActualizat.Nume!, id, cancellationToken);
            if (exista)
            {
                return Result<Produs>.Fail(
                    "Exista deja un produs cu acest nume",
                    ResultErrorType.Conflict);
                   
            }


            Produs? produsSalvat = await repository.ActualizeazaProdusAsync(id, produsActualizat, cancellationToken);
            if (produsSalvat == null)
            {
                return Result<Produs>.Fail(
                    "Produsul nu a putut fi actualizat.",
                    ResultErrorType.NotFound);
            }


            return Result<Produs>.Ok(produsSalvat);
        }

        public async Task<Result<bool>> StergeProdusAsync(int id, CancellationToken cancellationToken)
        {
            Produs? produs = await repository.ObtineProdusAsync(id, cancellationToken);
            if (produs == null)
            {
                return Result<bool>.Fail(
                    "Produsul nu exista",
                    ResultErrorType.NotFound);
            }
            bool sters = await repository.StergeProdusAsync(id, cancellationToken);
            
            return Result<bool>.Ok(sters);  

        }
        
    }
}
