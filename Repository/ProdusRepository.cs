using Microsoft.EntityFrameworkCore;
using MyApi.Common;
using MyApi.Data;
using MyApi.DTOs.Produs;
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

        public async Task<PagedResult<ProdusDto>> ObtineToateProduseleAsync
            (ProdusFiltruDto filtru,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            IQueryable<ProdusDto> query = context.Produse
                .AsNoTracking()
                .Select(p => new ProdusDto
                {
                    Id = p.Id,
                    Nume = p.Nume,
                    Pret = p.Pret,
                    CategorieId = p.CategorieId,
                    NumeCategorie = p.Categorie!.Nume,
                    RowVersion = p.RowVersion
                });

            if (filtru.CategorieId.HasValue)
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

            if(filtru.SortBy == "pret")
            {
                query = filtru.Descending
                    ? query.OrderByDescending(p => p.Pret) 
                    : query.OrderBy(p => p.Pret);
            }
            else if( filtru.SortBy == "nume")
            {
                query = filtru.Descending
                    ? query.OrderByDescending(p => p.Nume)
                    : query.OrderBy(p => p.Nume);
            }
            else
            {
                query = query.OrderBy(p => p.Id);
            }

                int totalCount = await query.CountAsync(cancellationToken);
            int totalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize);

            int skip = (page - 1) * pageSize;
            List<ProdusDto> produseDto = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<ProdusDto>
            {
                Data = produseDto,
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
            context.Produse.Add(produsNou);
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
            
            //pt optimistic concurrency
            context.Entry(produs)
                .Property(p => p.RowVersion)
                .OriginalValue = produsActualizat.RowVersion;

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
