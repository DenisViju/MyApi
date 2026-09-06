using MyApi.Repository;
using MyApi.Common;
using MyApi.DTOs.Adresa;
using MyApi.Models;


namespace MyApi.Services
{
    public class AdresaService : IAdresaService
    {
        private readonly IAdresaRepository repository;

        public AdresaService(IAdresaRepository repository)
        {
            this.repository = repository;   
        }

        public async Task<Result<List<AdresaDto>>> ObtineAdreseleUseruluiAsync(int userId, CancellationToken cancellationToken)
        {
            List<Adresa> adrese = await repository.ObtineAdreseleUseruluiAsync(userId, cancellationToken);
            var adreseDto = new List<AdresaDto>();

            foreach(var adresa in adrese)
            {
                adreseDto.Add(Mappers.AdresaMapper.ToDto(adresa));
            }
            return Result<List<AdresaDto>>.Ok(adreseDto);
        }

        public async Task<Result<AdresaDto>> ObtineAdresaAsync(int id, int userId, CancellationToken cancellationToken)
        {
            Adresa? adresa = await repository.ObtineAdresaAsync(id, userId, cancellationToken);
            if(adresa == null)
            {
                return Result<AdresaDto>.Fail(
                    "Adresa nu exista",
                    Enums.ResultErrorType.NotFound);
            }

            return Result<AdresaDto>.Ok(Mappers.AdresaMapper.ToDto(adresa));
        }

        public async Task<Result<AdresaDto>> AdaugaAdresaAsync
            (AdresaCreateDto adresaCreateDto, int userId, CancellationToken cancellationToken)
        {
            Adresa adresaNoua = Mappers.AdresaMapper.ToEntity(adresaCreateDto, userId);

            if(adresaNoua.EstePrincipala)
            {
                Adresa? adresaPrincipalaActuala = await repository.ObtineAdresaPrincipalaAsync(userId, cancellationToken);
                if(adresaPrincipalaActuala != null)
                {
                    adresaPrincipalaActuala.EstePrincipala = false;
                }
            }

            Adresa adresaSalvata = await repository.CreeazaAdresaAsync(adresaNoua, cancellationToken);

            return Result<AdresaDto>.Ok(Mappers.AdresaMapper.ToDto(adresaSalvata));
            
        }

        public async Task<Result<AdresaDto>> ActualizeazaAdresaAsync
            (AdresaUpdateDto adresaUpdateDto, int id, int userId, CancellationToken cancellationToken)
        {
            Adresa? adresa = await repository.ObtineAdresaAsync(id, userId, cancellationToken);
            if (adresa == null)
            {
                return Result<AdresaDto>.Fail(
                    "Adresa nu exista",
                    Enums.ResultErrorType.NotFound);
            }

            if (adresaUpdateDto.EstePrincipala)
            {
                Adresa? adresaPrincipalaActuala = await repository.ObtineAdresaPrincipalaAsync(userId, cancellationToken);
                if (adresaPrincipalaActuala != null && adresaPrincipalaActuala.Id != adresa.Id)
                {
                    adresaPrincipalaActuala.EstePrincipala = false;
                }
            }


            Mappers.AdresaMapper.UpdateEntity(adresa, adresaUpdateDto);
            await repository.SalveazaSchimbarileAsync(cancellationToken);

            return Result<AdresaDto>.Ok(Mappers.AdresaMapper.ToDto(adresa));
            
        }

        public async Task<Result<bool>> StergeAdresaAsync(int id, int userId, CancellationToken cancellationToken)
        {
            Adresa? adresa = await repository.ObtineAdresaAsync(id, userId, cancellationToken);
            if (adresa == null)
            {
                return Result<bool>.Fail(
                    "Adresa nu exista",
                    Enums.ResultErrorType.NotFound);
            }

            if(adresa.EstePrincipala)
            {
                Adresa? nouaAdresaPrincipala = await repository
                    .ObtineAltaAdresaAsync(adresa.Id, userId, cancellationToken);

                if(nouaAdresaPrincipala != null)
                {
                    nouaAdresaPrincipala.EstePrincipala = true;
                }
            }

            await repository.StergeAdresaAsync(adresa, cancellationToken);

            return Result<bool>.Ok(true);
        }

        

    }
}
