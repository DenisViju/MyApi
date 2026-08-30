using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.Models;
using MyApi.DTOs;
using MyApi.Common;

namespace MyApi.Repository
{
    public class ProdusRepository : IProdusRepository
    {
        private readonly AplicatieDbContext context;
        public ProdusRepository(AplicatieDbContext context)
        {
            this.context = context;
        }

        public async Task<PagedResult<Produs>> ObtineToateProduseleAsync
            (ProdusFiltruDto filtru,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            IQueryable<Produs> query = context.Produse
                .AsNoTracking()
                .Include(p => p.Categorie);

            if(filtru.CategorieId.HasValue)
            {
                query = query.Where(p => p.CategorieId == filtru.CategorieId.Value);
            }

            if(filtru.PretMinim.HasValue)
            {
                query = query.Where(p => p.Pret >= filtru.PretMinim.Value);
            }
            if (filtru.PretMaxim.HasValue)
            {
                query = query.Where(p => p.Pret <= filtru.PretMaxim.Value);
            }
            if(!string.IsNullOrWhiteSpace(filtru.Nume))
            {
                query = query.Where(p => p.Nume!.Contains(filtru.Nume));
            }

            int totalCount = await query.CountAsync(cancellationToken);
            int totalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize);

            int skip = (page - 1) * pageSize;
            List<Produs> produse = await query
                .OrderBy(p => p.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Produs>
            {
                Data = produse,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
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
