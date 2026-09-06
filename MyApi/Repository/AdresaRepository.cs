using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.Models;

namespace MyApi.Repository
{
    public class AdresaRepository : IAdresaRepository
    {
        private readonly AplicatieDbContext context;

        public AdresaRepository(AplicatieDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Adresa>> ObtineAdreseleUseruluiAsync(int userId, CancellationToken cancellationToken)
        {
            return await context.Adrese
                .AsNoTracking()
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.EstePrincipala)
                .ThenBy(a => a.Id)
                .ToListAsync(cancellationToken);
        }  

        public async Task<Adresa?> ObtineAdresaAsync(int id, int userId, CancellationToken cancellationToken)
        {
            return await context.Adrese
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId, cancellationToken);
        }
        public async Task<Adresa?> ObtineAdresaPrincipalaAsync(int userId, CancellationToken cancellationToken)
        {
            return await context.Adrese
                .FirstOrDefaultAsync(a => a.UserId == userId && a.EstePrincipala, cancellationToken);
        }
        public async Task<Adresa?> ObtineAltaAdresaAsync(int id, int userId, CancellationToken cancellationToken)
        {
            return await context.Adrese
                .FirstOrDefaultAsync(a => a.UserId == userId && a.Id != id, cancellationToken);
        }


        public async Task<Adresa> CreeazaAdresaAsync(Adresa adresaNoua, CancellationToken cancellationToken)
        {
            context.Adrese.Add(adresaNoua);

            await context.SaveChangesAsync(cancellationToken);

            return adresaNoua;
        }

        public async Task StergeAdresaAsync(Adresa adresa, CancellationToken cancellationToken)
        {
            context.Adrese.Remove(adresa);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task SalveazaSchimbarileAsync(CancellationToken cancellationToken)
        {
            await context.SaveChangesAsync(cancellationToken);
        }


    }
}
