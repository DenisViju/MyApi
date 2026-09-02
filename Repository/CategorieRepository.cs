using Microsoft.EntityFrameworkCore;
using MyApi.Common;
using MyApi.Data;
using MyApi.DTOs.Categorie;
using MyApi.Models;
using System.Threading;

namespace MyApi.Repository
{
    public class CategorieRepository : ICategorieRepository
    {
        private readonly AplicatieDbContext context;

        public CategorieRepository(AplicatieDbContext context)
        {
            this.context = context;
        }

        public async Task<PagedResult<CategorieDto>> ObtineCategoriiAsync
            (CategorieFiltruDto filtru, int page, int pageSize, CancellationToken cancellationToken)
        {
            IQueryable<CategorieDto> query = context.Categorii
                .AsNoTracking()
                .Select(c => new CategorieDto
                {
                    Id = c.Id,
                    Nume = c.Nume,
                    RowVersion = c.RowVersion
                });

            if(!string.IsNullOrWhiteSpace(filtru.Nume))
            {
                query = query.Where(c => c.Nume!.Contains(filtru.Nume));
            }

            if(filtru.Descending)
            {
                query = query.OrderByDescending(c => c.Nume);   
            }
            else
            {
                query = query.OrderBy(c => c.Nume);
            }

            int totalCount = await query.CountAsync(cancellationToken);
            int totalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize);

            int skip = (page - 1) * pageSize;
            List<CategorieDto> categoriiDto= await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

           
            return new PagedResult<CategorieDto>
            {
                Data = categoriiDto,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages

            };

        }


        public async Task<Categorie?> ObtineCategorieAsync(int id, CancellationToken cancellationToken)
        {
            return await context.Categorii
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Categorie> CreeazaCategorieAsync(Categorie categorieNoua, CancellationToken cancellationToken)
        {
            context.Categorii.Add(categorieNoua);
            await context.SaveChangesAsync(cancellationToken);

            return categorieNoua;
        }

        public async Task<Categorie?> ActualizeazaCategorieAsync
            (int id, Categorie categorieActualizata, CancellationToken cancellationToken)
        {
            Categorie? categorie = await context.Categorii
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (categorie == null)
            {
                return null;
            }

            context.Entry(categorie)
                .Property(c => c.RowVersion)
                .OriginalValue = categorieActualizata.RowVersion;

            categorie.Nume = categorieActualizata.Nume;

            await context.SaveChangesAsync(cancellationToken);

            return categorie;
            
        }

        public async Task StergeCategorieAsync(Categorie categorie, CancellationToken cancellationToken)
        {
           
            context.Categorii.Remove(categorie);
            await context.SaveChangesAsync(cancellationToken);


        }
        public async Task<bool> ExistaCategorieCuNumeleAsync(string nume, CancellationToken cancellationToken)
        {
            return await context.Categorii.AnyAsync(c => c.Nume == nume, cancellationToken);
        }

        public async Task<bool> ExistaAltaCategorieCuNumeleAsync(string nume, int id, CancellationToken cancellationToken)
        {
            return await context.Categorii
                .AnyAsync(
                    c => c.Nume == nume && c.Id != id,
                    cancellationToken);
        }

        public async Task<Categorie?> ObtineCategorieCuProduseAsync(int id, CancellationToken cancellationToken)
        {
            return await context.Categorii
                .Include(c => c.Produse)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
    }
}
