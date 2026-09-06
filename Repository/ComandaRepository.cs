using Microsoft.EntityFrameworkCore;
using MyApi.Common;
using MyApi.Data;
using MyApi.DTOs.Comanda;
using MyApi.Models;

namespace MyApi.Repository
{
    public class ComandaRepository : IComandaRepository
    {
        private readonly AplicatieDbContext context;

        public ComandaRepository(AplicatieDbContext context)
        {
            this.context = context;
        }

       public async Task<List<Comanda>> ObtineComenzileUseruluiAsync(int userId, CancellationToken cancellationToken)
        {
            return await context.Comenzi
                .AsNoTracking()
                .Include(c => c.ElementeComanda)
                    .ThenInclude(e => e.Produs)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.DataCrearii)
                .ToListAsync(cancellationToken);

        }

        public async Task<PagedResult<ComandaDto>> ObtineToateComenzileAsync(
             ComandaFiltruDto filtru,
             int page,
             int pageSize,
             CancellationToken cancellationToken)
        {
            IQueryable<Comanda> query = context.Comenzi
                .AsNoTracking();

            if (filtru.Status.HasValue)
            {
                query = query.Where(c => c.Status == filtru.Status.Value);
            }

            if (filtru.UserId.HasValue)
            {
                query = query.Where(c => c.UserId == filtru.UserId.Value);
            }

            if (filtru.DataDeLa.HasValue)
            {
                var dataDeLa = filtru.DataDeLa.Value.Date;

                query = query.Where(c => c.DataCrearii >= dataDeLa);
            }

            if (filtru.DataPanaLa.HasValue)
            {
                var dataPanaLaExclusiva = filtru.DataPanaLa.Value.Date.AddDays(1);

                query = query.Where(c => c.DataCrearii < dataPanaLaExclusiva);
            }

            if (filtru.SortBy?.ToLower() == "total")
            {
                query = filtru.Descending
                    ? query.OrderByDescending(c => c.Total)
                           .ThenByDescending(c => c.Id)
                    : query.OrderBy(c => c.Total)
                           .ThenBy(c => c.Id);
            }
            else if (filtru.SortBy?.ToLower() == "status")
            {
                query = filtru.Descending
                    ? query.OrderByDescending(c => c.Status)
                           .ThenByDescending(c => c.Id)
                    : query.OrderBy(c => c.Status)
                           .ThenBy(c => c.Id);
            }
            else
            {
                query = filtru.Descending
                    ? query.OrderByDescending(c => c.DataCrearii)
                           .ThenByDescending(c => c.Id)    
                    : query.OrderBy(c => c.DataCrearii)
                           .ThenBy(c => c.Id);
            }

            int totalCount = await query.CountAsync(cancellationToken);
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            int skip = (page - 1) * pageSize;

            List<ComandaDto> comenziDto = await query
                .Skip(skip)
                .Take(pageSize)
                .Select(c => new ComandaDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Strada = c.Strada,
                    Oras = c.Oras,
                    Judet = c.Judet,
                    Tara = c.Tara,
                    CodPostal = c.CodPostal,
                    NumeDestinatar = c.NumeDestinatar,
                    TelefonDestinatar = c.TelefonDestinatar,
                    DataCrearii = c.DataCrearii,
                    Status = c.Status,
                    Total = c.Total,
                    RowVersion = c.RowVersion,
                    ElementeComanda = c.ElementeComanda.Select(ec => new ElementComandaDto
                    {
                        Id = ec.Id,
                        ProdusId = ec.ProdusId,
                        NumeProdus = ec.Produs!.Nume,
                        PretUnitar = ec.PretUnitar,
                        Cantitate = ec.Cantitate
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<ComandaDto>
            {
                Data = comenziDto,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }

        public async Task<Comanda?> ObtineComandaAsync(int id, int userId, CancellationToken cancellationToken)
        {
            return await context.Comenzi
                .Include(c => c.ElementeComanda)
                    .ThenInclude(e => e.Produs)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Id == id, cancellationToken);
                
        }

        public async Task<Comanda?> ObtineOriceComandaAsync(int id, CancellationToken cancellationToken)
        {
            return await context.Comenzi
                .Include(c => c.ElementeComanda)
                    .ThenInclude(e => e.Produs)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        

        public async Task<Comanda> CreeazaComandaAsync(Comanda comanda, CancellationToken cancellationToken)
        {
            context.Add(comanda);
            await context.SaveChangesAsync(cancellationToken);

            return comanda;
        }

        public async Task SalveazaSchimbarileAsync(CancellationToken cancellationToken)
        {
            await context.SaveChangesAsync(cancellationToken);
        }

    }
}
