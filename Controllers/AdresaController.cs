using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApi.DTOs.Adresa;
using MyApi.Services;

namespace MyApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdresaController : BaseController
    {
        private readonly IAdresaService service;

        public AdresaController(IAdresaService service)
        {
            this.service = service;
        }

        [HttpGet("adrese")]
        public async Task<ActionResult<List<AdresaDto>>> ObtineAdrese(CancellationToken cancellationToken)
        {
            var userId = ObtineUserId();
            if(userId == -1)
            {
                return Unauthorized();
            }

            var result = await service.ObtineAdreseleUseruluiAsync(userId, cancellationToken);

            if (!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdresaDto>> ObtineAdresa(int id, CancellationToken cancellationToken)
        {
            var userId = ObtineUserId();
            if (userId == -1)
            {
                return Unauthorized();
            }

            var result = await service.ObtineAdresaAsync(id, userId, cancellationToken);

            if (!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<ActionResult<AdresaDto>> AdaugaAdresa
            ([FromBody] AdresaCreateDto adresaCreateDto, CancellationToken cancellationToken)
        {
            var userId = ObtineUserId();
            if (userId == -1)
            {
                return Unauthorized();
            }

            var result = await service.AdaugaAdresaAsync(adresaCreateDto, userId, cancellationToken);

            if (!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return CreatedAtAction(nameof(ObtineAdresa), new { id = result.Data!.Id }, result.Data!);

        }

        [HttpPut("{id}")] 
        public async Task<ActionResult<AdresaDto>> ActualizeazaAdresa
            ([FromBody] AdresaUpdateDto adresaUpdateDto, int id,  CancellationToken cancellationToken)
        {
            var userId = ObtineUserId();
            if (userId == -1)
            {
                return Unauthorized();
            }

            var result = await service.ActualizeazaAdresaAsync(adresaUpdateDto, id, userId, cancellationToken);

            if (!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return Ok(result.Data);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> StergeAdresa(int id, CancellationToken cancellationToken)
        {
            var userId = ObtineUserId();
            if (userId == -1)
            {
                return Unauthorized();
            }

            var result = await service.StergeAdresaAsync(id, userId, cancellationToken);

            if (!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return NoContent();
        }

    }
}
