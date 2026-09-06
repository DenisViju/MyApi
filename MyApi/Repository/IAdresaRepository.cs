using MyApi.Models;

namespace MyApi.Repository
{
    public interface IAdresaRepository
    {
        Task<List<Adresa>> ObtineAdreseleUseruluiAsync(int userId, CancellationToken cancellationToken);
        Task<Adresa?> ObtineAdresaAsync(int id, int userId, CancellationToken cancellationToken);
        Task<Adresa?> ObtineAdresaPrincipalaAsync(int userId, CancellationToken cancellationToken);
        Task<Adresa?> ObtineAltaAdresaAsync(int id, int userId, CancellationToken cancellationToken);
        Task<Adresa> CreeazaAdresaAsync(Adresa adresaNoua, CancellationToken cancellationToken);
        Task StergeAdresaAsync(Adresa adresa, CancellationToken cancellationToken);
        Task SalveazaSchimbarileAsync(CancellationToken cancellationToken);
        
    }
}
