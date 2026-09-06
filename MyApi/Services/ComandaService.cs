using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyApi.Common;
using MyApi.DTOs.Comanda;
using MyApi.Models;
using MyApi.Repository;
using System.Data;

namespace MyApi.Services
{
    public class ComandaService : IComandaService
    {
        private readonly IComandaRepository comandaRepository;
        private readonly IAdresaRepository adresaRepository;
        private readonly IProdusRepository produsRepository;

        public ComandaService(
            IComandaRepository comandaRepository,
            IAdresaRepository adresaRepository,
            IProdusRepository produsRepository)
        {
            this.comandaRepository = comandaRepository;
            this.adresaRepository = adresaRepository;
            this.produsRepository = produsRepository;
        }

        public async Task<Result<ComandaDto>> CreeazaComandaAsync
            (ComandaCreateDto comandaCreateDto, int userId, CancellationToken cancellationToken)
        {
            //verificam sa fie elemente in comanda
            if (comandaCreateDto.ElementeComandaCreateDto.IsNullOrEmpty())
            {
                return Result<ComandaDto>.Fail(
                    "Comanda trebuie sa contina cel putin un produs",
                    Enums.ResultErrorType.BadRequest);
            }

            //obtinem adresa dupa Id si verificam ca aceasta sa existe
            Adresa? adresa = await adresaRepository
                .ObtineAdresaAsync(comandaCreateDto.AdresaId, userId, cancellationToken);
            if (adresa == null)
            {
                return Result<ComandaDto>.Fail(
                    "Adresa la care vrei sa faci comanda nu exista",
                    Enums.ResultErrorType.NotFound);
            }

            if (comandaCreateDto.ElementeComandaCreateDto
                .GroupBy(x => x.ProdusId)
                .Any(g => g.Count() > 1))
            {
                return Result<ComandaDto>.Fail(
                   "Ai introdus acelasi produs de mai multe ori",
                   Enums.ResultErrorType.BadRequest);
            }

            var produseIds = comandaCreateDto.ElementeComandaCreateDto
                .Select(e => e.ProdusId)
                .Distinct()
                .ToList();

            var produse = await produsRepository
                 .ObtineProduseleAsync(produseIds, cancellationToken);

            var produseGasiteIds = produse
            .Select(p => p.Id)
            .ToList();

            var produseLipsa = produseIds
                .Except(produseGasiteIds)
                .ToList();

            if (produseLipsa.Count > 0)
            {
                return Result<ComandaDto>.Fail(
                    $"Exista cel putin un produs in comanda care nu a fost gasit: {string.Join(", ", produseLipsa)}",
                    Enums.ResultErrorType.NotFound);
            }

            var produseDictionary = produse.ToDictionary(p => p.Id);



            //transformam lista de elementeDto in elemente
            //ca sa facem asta o sa transformam pe rand fiecare element din lista cu ElementComandaMapper.ToEntity..
            decimal total = 0;
            var elementeComanda = new List<ElementComanda>();
            foreach (var elementComandaCreateDto in comandaCreateDto.ElementeComandaCreateDto)
            {
                if (elementComandaCreateDto.Cantitate <= 0)
                {
                    return Result<ComandaDto>.Fail(
                        "Cantitatea trebuie sa fie mai mare decat 0",
                        Enums.ResultErrorType.BadRequest);
                }
                //..pentru asta avem nevoie de produsul fiecarui element 
                var produs = produseDictionary[elementComandaCreateDto.ProdusId];

                //verificam daca stocul este suficient
                if (produs.Stoc < elementComandaCreateDto.Cantitate)
                {
                    return Result<ComandaDto>.Fail(
                        $"Stoc insuficient pentru produsul '{produs.Nume}'. Stoc disponibil: {produs.Stoc}"
                        , Enums.ResultErrorType.BadRequest);
                }
                //daca esti modificam stocul si adaugam la total valoarea elementului curent
                produs.Stoc -= elementComandaCreateDto.Cantitate;
                total += produs.Pret * elementComandaCreateDto.Cantitate;

                elementeComanda.Add(Mappers.ElementComandaMapper.ToEntity(elementComandaCreateDto, produs));

            }

            Comanda comanda = Mappers.ComandaMapper.ToEntity
                (adresa, elementeComanda, userId, DateTime.UtcNow, total);
            Comanda comandaSalvata;
            //incercam sa adaugam comanda in baza de date
            //CreeazaComandaAsync foloseste SaveChangesAsync() care poate arunca o exceptie 
            //daca avem un rowVersion diferit pentru produs
            //in plus nu avem nevoie de transaction manual deoarece toate modificarile sunt urmarite si trimise printr-un singur SaveChangesAsync()
            try
            {
                comandaSalvata = await comandaRepository.CreeazaComandaAsync(comanda, cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<ComandaDto>.Fail(
                    "Unul dintre produse a fost modificat sau stocul s-a epuizat intre timp. Te rugam sa reincerci.",
                     Enums.ResultErrorType.Conflict);
            }
            //trandformam in dto ca sa trimitem mai departe controller ului
            var elementeComandaDto = comandaSalvata.ElementeComanda
                .Select(Mappers.ElementComandaMapper.ToDto)
                .ToList();

            return Result<ComandaDto>.Ok(Mappers.ComandaMapper.ToDto(comandaSalvata, elementeComandaDto));


        }

        public async Task<Result<List<ComandaDto>>> ObtineComenzileUseruluiAsync
            (int userId, CancellationToken cancellationToken)
        {
            List<Comanda> comenzi = await comandaRepository.ObtineComenzileUseruluiAsync(userId, cancellationToken);
            var elementeComandaDto = new List<ElementComandaDto>();
            var comenziDto = new List<ComandaDto>();
            foreach (Comanda comanda in comenzi)
            {
                elementeComandaDto = comanda.ElementeComanda
                    .Select(Mappers.ElementComandaMapper.ToDto)
                    .ToList();

                comenziDto.Add(Mappers.ComandaMapper.ToDto(comanda, elementeComandaDto));
            }

            return Result<List<ComandaDto>>.Ok(comenziDto);
        }

        public async Task<Result<ComandaDto>> ObtineComandaAsync(int id, int userId, CancellationToken cancellationToken)
        {
            Comanda? comanda = await comandaRepository.ObtineComandaAsync(id, userId, cancellationToken);
            if (comanda == null)
            {
                return Result<ComandaDto>.Fail(
                    "Comanda nu exista",
                    Enums.ResultErrorType.NotFound);
            }

            var elementeComandaDto = comanda.ElementeComanda
                    .Select(Mappers.ElementComandaMapper.ToDto)
                    .ToList();

            return Result<ComandaDto>.Ok(Mappers.ComandaMapper.ToDto(comanda, elementeComandaDto));

        }

        public async Task<Result<ComandaDto>> AnuleazaComandaAsync(int id, int userId, CancellationToken cancellationToken)
        {
            Comanda? comanda = await comandaRepository.ObtineComandaAsync(id, userId, cancellationToken);
            if (comanda == null)
            {
                return Result<ComandaDto>.Fail(
                    "Comanda nu exista",
                    Enums.ResultErrorType.NotFound);
            }

            if (comanda.Status != Enums.StatusComanda.Noua)
            {
                return Result<ComandaDto>.Fail(
                    "Comanda nu mai poate fi anulata",
                    Enums.ResultErrorType.BadRequest);
            }

            comanda.Status = Enums.StatusComanda.Anulata;
            try
            {
                await comandaRepository.SalveazaSchimbarileAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<ComandaDto>.Fail(
                    "Comanda a fost modificata intre timp. Te rugam sa reincerci.",
                     Enums.ResultErrorType.Conflict);
            }

            var elementeComandaDto = comanda.ElementeComanda
                    .Select(Mappers.ElementComandaMapper.ToDto)
                    .ToList();

            return Result<ComandaDto>.Ok(Mappers.ComandaMapper.ToDto(comanda, elementeComandaDto));
        }

        public async Task<Result<PagedResult<ComandaDto>>> ObtineToateComenzileAsync
            (ComandaFiltruDto filtru, int page, int pageSize, CancellationToken cancellationToken)
        {
            if (page < 1)
            {
                return Result<PagedResult<ComandaDto>>.Fail(
                    "Pagina trebuie sa fie cel putin 1.",
                    Enums.ResultErrorType.BadRequest);
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return Result<PagedResult<ComandaDto>>.Fail(
                    "pageSize trebuie sa fie intre 1 și 100.",
                    Enums.ResultErrorType.BadRequest
                    );

            }

            PagedResult<ComandaDto> pagedResult = await comandaRepository
                .ObtineToateComenzileAsync(filtru, page, pageSize, cancellationToken);

            return Result<PagedResult<ComandaDto>>.Ok(pagedResult);
        }

        public async Task<Result<ComandaDto>> ObtineOriceComandaAsync(int id, CancellationToken cancellationToken)
        {
            Comanda? comanda = await comandaRepository.ObtineOriceComandaAsync(id, cancellationToken);

            if (comanda == null)
            {
                return Result<ComandaDto>.Fail(
                    "Comanda nu exista",
                    Enums.ResultErrorType.NotFound);
            }

            var elementeComandaDto = comanda.ElementeComanda
                .Select(Mappers.ElementComandaMapper.ToDto)
                .ToList();

            return Result<ComandaDto>.Ok(Mappers.ComandaMapper.ToDto(comanda, elementeComandaDto));


        }


        public async Task<Result<ComandaDto>> ActualizeazaStatusComandaAsync
            (int id, ComandaStatusUpdateDto comandaStatusUpdateDto, CancellationToken cancellationToken)
        {
            Comanda? comanda = await comandaRepository.ObtineOriceComandaAsync(id, cancellationToken);

            if (comanda == null)
            {
                return Result<ComandaDto>.Fail(
                    "Comanda nu exista",
                    Enums.ResultErrorType.NotFound);
            }

            if (!EstePermisaTranzitia(comanda.Status, comandaStatusUpdateDto.Status))
            {
                return Result<ComandaDto>.Fail(
                    "Tranzitia la noul status nu este permisa",
                    Enums.ResultErrorType.BadRequest);
            }

            comanda.Status = comandaStatusUpdateDto.Status;

            try
            {
                await comandaRepository.SalveazaSchimbarileAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<ComandaDto>.Fail(
                    "Comanda a fost modificata intre timp. Te rugam sa reincerci.",
                     Enums.ResultErrorType.Conflict);
            }

            var elementeComandaDto = comanda.ElementeComanda
                    .Select(Mappers.ElementComandaMapper.ToDto)
                    .ToList();

            return Result<ComandaDto>.Ok(Mappers.ComandaMapper.ToDto(comanda, elementeComandaDto));
        }
        private bool EstePermisaTranzitia(Enums.StatusComanda statusActual, Enums.StatusComanda statusNou)
        {
            if (statusActual == Enums.StatusComanda.Noua
                && (statusNou == Enums.StatusComanda.Anulata || statusNou == Enums.StatusComanda.Confirmata))
            {
                return true;
            }

            if (statusActual == Enums.StatusComanda.Platita && statusNou == Enums.StatusComanda.Expediata)
            {
                return true;
            }

            if (statusActual == Enums.StatusComanda.Expediata && statusNou == Enums.StatusComanda.Livrata)
            {
                return true;
            }

            return false;
        }




    }
}
