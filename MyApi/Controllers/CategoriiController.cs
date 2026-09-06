using Microsoft.AspNetCore.Mvc;
using MyApi.Common;
using MyApi.DTOs.Categorie;
using MyApi.Services;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriiController : BaseController
    {
        private readonly ICategorieService service;
        public CategoriiController(ICategorieService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<CategorieDto>>> ObtineCategorii
            ([FromQuery] CategorieFiltruDto filtru, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            Result<PagedResult<CategorieDto>> result = await service
                .ObtineCategoriiAsync(filtru, page, pageSize, cancellationToken);

            if(!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return Ok(result.Data!);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategorieDto>> ObtineCategorie(int id, CancellationToken cancellationToken)
        {
            Result<CategorieDto> result = await service.ObtineCategorieAsync(id, cancellationToken);

            if (!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return Ok(result.Data);

        }

        [HttpPost]
        public async Task<ActionResult<CategorieDto>> CreeazaCategorie
            ([FromBody] CategorieCreateDto categorieCreateDto, CancellationToken cancellationToken)
        {
            var result = await service.AdaugaCategorieAsync(categorieCreateDto, cancellationToken);
            
            if(!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return CreatedAtAction(nameof(ObtineCategorie), new { id = result.Data!.Id }, result.Data);

        }


        [HttpPut("{id}")]
        public async Task<ActionResult<CategorieDto>> ActualizeazaCategorie
            (int id, [FromBody] CategorieUpdateDto categorieUpdateDto, CancellationToken cancellationToken)
        {
            var result = await service.ActualizeazaCategorieAsync (id, categorieUpdateDto, cancellationToken);

            if(!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> StergeCategorie(int id, CancellationToken cancellationToken)
        {
            var result = await service.StergeCategorieAsync(id, cancellationToken);   
            if(!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }
            return NoContent();
        }

    }
}
