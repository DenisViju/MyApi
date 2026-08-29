using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.Models;

namespace MyApi.Repository
{
    public class ProdusRepository : IProdusRepository
    {
        private readonly AplicatieDbContext context;
        public ProdusRepository(AplicatieDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Produs>> ObtineToateProduseleAsync(CancellationToken cancellationToken)
        {
            return await context.Produse
                .AsNoTracking()
                .Include(p => p.Categorie)
                .ToListAsync(cancellationToken);
        }

        public async Task<Produs?> ObtineProdusAsync(int id, CancellationToken cancellationToken)
        {
            return await context.Produse
                .AsNoTracking()
                .Include(p => p.Categorie)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Produs> AdaugaProdusAsync(Produs produsNou, CancellationToken cancellationToken)
        {
            await context.Produse.AddAsync(produsNou, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return produsNou;
        }

        public async Task<Produs?> ActualizeazaProdusAsync
            (int id, Produs produsActualizat, CancellationToken cancellationToken)
        {
            Produs? produs = await context.Produse
                .Include(p => p.Categorie)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (produs == null)
                return null;

            produs.Nume = produsActualizat.Nume;
            produs.Pret = produsActualizat.Pret;
            produs.CategorieId = produsActualizat.CategorieId;

            await context.SaveChangesAsync(cancellationToken);

            return produs;

        }

        public async Task<bool> StergeProdusAsync(int id, CancellationToken cancellationToken)
        {
            Produs? produs = await context.Produse
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if(produs == null)
                return false;

            context.Produse.Remove(produs);
            await context.SaveChangesAsync(cancellationToken);

            return true;

        }

        public async Task<Categorie?> GasesteCategorieAsync(int id, CancellationToken cancellationToken)
        {
            return await context.Categorii
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);   
        }

        public async Task<bool> ExistaProdusCuNumeleAsync(string nume, CancellationToken cancellationToken)
        {
            return await context.Produse
                .AsNoTracking()
                .AnyAsync(p => p.Nume == nume, cancellationToken); 
        }
        public async Task<bool> ExistaAltProdusCuNumeleAsync(string nume, int id, CancellationToken cancellationToken)
        {
            return await context.Produse
                .AsNoTracking()
                .AnyAsync(p => p.Nume == nume && p.Id != id, cancellationToken);
        }

    }
}
