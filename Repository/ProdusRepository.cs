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

        public async Task<List<Produs>> ObtineToateProduseleAsync()
        {
            return await context.Produse
                .Include(p => p.Categorie)
                .ToListAsync();
        }

        public async Task<Produs?> ObtineProdusAsync(int id)
        {
            return await context.Produse
                .Include(p => p.Categorie)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Produs> AdaugaProdusAsync(Produs produsNou)
        {
            await context.Produse.AddAsync(produsNou);
            await context.SaveChangesAsync();

            return produsNou;
        }

        public async Task<Produs?> ActualizeazaProdusAsync(int id, Produs produsActualizat)
        {
            Produs? produs = await context.Produse
                .Include(p => p.Categorie)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produs == null)
                return null;

            produs.Nume = produsActualizat.Nume;
            produs.Pret = produsActualizat.Pret;
            produs.CategorieId = produsActualizat.CategorieId;

            await context.SaveChangesAsync();

            return produs;

        }

        public async Task<bool> StergeProdusAsync(int id)
        {
            Produs? produs = await context.Produse
                .FirstOrDefaultAsync(p => p.Id == id);

            if(produs == null)
                return false;

            context.Produse.Remove(produs);
            await context.SaveChangesAsync();

            return true;

        }

        public async Task<Categorie?> GasesteCategorieAsync(int id)
        {
            return await context.Categorii
                .FirstOrDefaultAsync(c => c.Id == id);   
        }

        public async Task<bool> ExistaProdusCuNumeleAsync(string nume)
        {
            return await context.Produse
                .AnyAsync(p => p.Nume == nume); 
        }
        public async Task<bool> ExistaAltProdusCuNumeleAsync(string nume, int id)
        {
            return await context.Produse
                .AnyAsync(p => p.Nume == nume && p.Id != id);
        }

    }
}
