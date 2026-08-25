using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.Models;
namespace MyApi.Services
{
    public class ProdusService : IProdusService
    {
        private readonly AplicatieDbContext context;
        public ProdusService(AplicatieDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Produs>> ObtineToateProdusele()
        {
           return await context.Produse.ToListAsync(); 
        }
        public async Task<Produs?> ObtineProdus(int id)
        {
            return await context.Produse.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Produs> AdaugaProdus(Produs produs)
        {
            await context.Produse.AddAsync(produs);
            await context.SaveChangesAsync();

            return produs;
        }

        public async Task<Produs?> ActualizeazaProdus(int id, Produs produsActualizat) 
        {
            Produs? produs = await context.Produse.FirstOrDefaultAsync(p => p.Id == id);
            if (produs == null)
            {
                return null;
            }
            produs.Nume = produsActualizat.Nume;
            produs.Pret = produsActualizat.Pret;
            await context.SaveChangesAsync();

            return produs;
        }

        public async Task<bool> StergeProdus(int id)
        {
            Produs? produs = await context.Produse.FirstOrDefaultAsync(p =>p.Id == id);
            if(produs == null)
            {
                return false;
            }
            context.Produse.Remove(produs);
            await context.SaveChangesAsync();

            return true;

        }
    }
}
