using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApi.Common;
using MyApi.DTOs.Comanda;
using MyApi.Services;

namespace MyApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ComandaController : BaseController
    {
        private readonly IComandaService service;

        public ComandaController(IComandaService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComandaDto>>> ObtineComenzi(CancellationToken cancellationToken)
        {
            int userId = ObtineUserId();
            if (userId == -1)
            {
                return Unauthorized();
            }

            var result = await service.ObtineComenzileUseruluiAsync(userId, cancellationToken);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComandaDto>> ObtineComanda(int id, CancellationToken cancellationToken)
        {
            int userId = ObtineUserId();
            if (userId == -1)
            {
                return Unauthorized();
            }

            var result = await service.ObtineComandaAsync(id, userId, cancellationToken);

            if (!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return Ok(result.Data);

        }

        [HttpPost]
        public async Task<ActionResult<ComandaDto>> CreeazaComanda
            ([FromBody] ComandaCreateDto comandaCreateDto, CancellationToken cancellationToken)
        {
            int userId = ObtineUserId();
            if (userId == -1)
            {
                return Unauthorized();
            }

            var result = await service.CreeazaComandaAsync(comandaCreateDto, userId, cancellationToken);

            if (!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return CreatedAtAction(nameof(ObtineComanda), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPost("{id}/anulare")]
        public async Task<ActionResult<ComandaDto>> AnuleazaComanda(int id, CancellationToken cancellationToken)
        {
            int userId = ObtineUserId();
            if (userId == -1)
            {
                return Unauthorized();
            }

            var result = await service.AnuleazaComandaAsync(id, userId, cancellationToken);

            if (!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return Ok(result.Data);
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedResult<ComandaDto>>> ObtineToateComenzile
            (ComandaFiltruDto filtru, int page, int pageSize, CancellationToken cancellationToken)
        {
            var result = await service.ObtineToateComenzileAsync(filtru, page, pageSize, cancellationToken);

            if (!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return Ok(result.Data);
        }

        [HttpGet("admin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ComandaDto>> ObtineOriceComanda(int id, CancellationToken cancellationToken)
        {
            var result = await service.ObtineOriceComandaAsync(id, cancellationToken);

            if (!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return Ok(result.Data);
        }

        [HttpPatch("admin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ComandaDto>> ActualizeazaStatusComanda
            (int id, [FromBody] ComandaStatusUpdateDto comandaStatusUpdateDto, CancellationToken cancellationToken)
        {
            var result = await service.ActualizeazaStatusComandaAsync
                (id, comandaStatusUpdateDto, cancellationToken);

            if (!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return Ok(result.Data);
        }



    }
}
