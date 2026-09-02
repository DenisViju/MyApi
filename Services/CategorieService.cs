using Microsoft.EntityFrameworkCore;
using MyApi.Common;
using MyApi.DTOs.Categorie;
using MyApi.Enums;
using MyApi.Models;
using MyApi.Repository;

namespace MyApi.Services
{
    public class CategorieService : ICategorieService
    {
        private readonly ICategorieRepository repository;

        public CategorieService(ICategorieRepository repository) 
        { 
            this.repository = repository;
        }

        public async Task<Result<PagedResult<CategorieDto>>> ObtineCategoriiAsync
            (CategorieFiltruDto filtru, int page, int pageSize, CancellationToken cancellationToken)
        {
            if (page <= 0)
            {
                return Result<PagedResult<CategorieDto>>.Fail(
                    "Pagina trebuie sa fie cel putin 1",
                    ResultErrorType.BadRequest);
            }
            if (pageSize < 1 || pageSize > 100)
            {
                return Result<PagedResult<CategorieDto>>.Fail(
                    "PageSize trebuie sa fie intre 1 si 100",
                    ResultErrorType.BadRequest);
            }

            PagedResult<CategorieDto> result = await repository
                .ObtineCategoriiAsync(filtru, page, pageSize, cancellationToken);

            return Result<PagedResult<CategorieDto>>.Ok(result);
        }

        public async Task<Result<CategorieDto>> ObtineCategorieAsync 
            (int id, CancellationToken cancellationToken)
        {
            Categorie? categorie = await repository.ObtineCategorieAsync(id, cancellationToken);

            if(categorie == null)
            {
                return Result<CategorieDto>.Fail(
                    "Nu exista aceasta categorie",
                    ResultErrorType.NotFound);
            }

            return Result<CategorieDto>.Ok(Mappers.CategorieMapper.ToDto(categorie));
        }

        public async Task<Result<CategorieDto>> AdaugaCategorieAsync
            (CategorieCreateDto categorieCreateDto, CancellationToken cancellationToken)
        {
            Categorie categorie = Mappers.CategorieMapper.ToEntity(categorieCreateDto);

            bool exista = await repository.ExistaCategorieCuNumeleAsync(categorie.Nume!, cancellationToken);
            if (exista)
            {
                return Result<CategorieDto>.Fail(
                    "Exista deja o categorie cu acest nume.",
                    ResultErrorType.Conflict);
            }
            Categorie categorieSalvata = await repository.CreeazaCategorieAsync(categorie, cancellationToken);

            return Result<CategorieDto>.Ok(Mappers.CategorieMapper.ToDto(categorieSalvata));

        }
        public async Task<Result<CategorieDto>> ActualizeazaCategorieAsync
            (int id, CategorieUpdateDto categorieUpdateDto, CancellationToken cancellationToken)
        {
           
            bool exista = await repository.ExistaAltaCategorieCuNumeleAsync(categorieUpdateDto.Nume!, id, cancellationToken);
            if (exista)
            {
                return Result<CategorieDto>.Fail(
                    "Exista deja o categorie cu acest nume.",
                    ResultErrorType.Conflict);
            }
            Categorie? categorieSalvata;
            try
            {
                categorieSalvata = await repository.ActualizeazaCategorieAsync(
                id,
                Mappers.CategorieMapper.ToEntity(categorieUpdateDto),
                cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<CategorieDto>.Fail(
                    "Categoria a fost modificata sau stearsa de un alt utilizator intre timp. " +
                    "Va rugam sa reincarcati pagina si sa incercati din nou.",
                    ResultErrorType.Conflict);
            }
            if (categorieSalvata == null)
            {
                return Result<CategorieDto>.Fail(
                    "Categoria nu a putut fi actualizata.",
                    ResultErrorType.NotFound);
            }

            return Result<CategorieDto>.Ok(Mappers.CategorieMapper.ToDto(categorieSalvata));
        }

        public async Task<Result<bool>> StergeCategorieAsync(int id, CancellationToken cancellationToken)
        {
            Categorie? categorie = await repository.ObtineCategorieCuProduseAsync(id, cancellationToken);
            if (categorie == null)
            {
                return Result<bool>.Fail(
                    "Categoria nu exista",
                    ResultErrorType.NotFound);
            }

            if(categorie.Produse.Any())
            {
                return Result<bool>.Fail(
                    "Categoria nu poate fi stearsa deoarece contine produse",
                    ResultErrorType.Conflict);
            }
            await repository.StergeCategorieAsync(categorie, cancellationToken);
            

            return Result<bool>.Ok(true);
        }
    }
}
