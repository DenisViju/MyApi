using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.DTOs;
using MyApi.Mappers;
using MyApi.Common;
using MyApi.Services;


namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProduseController : BaseController
    {
        private readonly IProdusService produsService;
        public ProduseController(IProdusService produsService)
        {
            this.produsService = produsService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ProdusDto>>> ObtineProduse
            ([FromQuery] ProdusFiltruDto filtru,
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await produsService.ObtineToateProduseleAsync(filtru, page, pageSize, cancellationToken);

            if (!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            PagedResult<ProdusDto> pagedResultDto = result.Data!;


            return Ok(pagedResultDto);

            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdusDto>> ObtineProdus(int id, CancellationToken cancellationToken)
        {
            var result = await produsService.ObtineProdusAsync(id, cancellationToken);

            if(!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return Ok( ProdusMapper.ToDto(result.Data!));


        }

        [HttpPost]
        public async Task<ActionResult<ProdusDto>> CreeazaProdus
            ([FromBody] ProdusCreateDto produsCreateDto, CancellationToken cancellationToken)
        {

            Produs produs = ProdusMapper.ToEntity(produsCreateDto);
         
            Result<Produs> result = 
                await produsService.AdaugaProdusAsync(produs, cancellationToken);

            if(!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }
            
            
            ProdusDto produsDto = 
                ProdusMapper.ToDto(result.Data!);

            return CreatedAtAction(nameof(ObtineProdus), new { id = produsDto.Id },produsDto );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProdusDto>> ActualizeazaProdus
            (int id, [FromBody] ProdusUpdateDto produsUpdateDto, CancellationToken cancellationToken)
        {
           
            Produs produsActualizat = ProdusMapper.ToEntity(produsUpdateDto);

            Result<Produs> result = await produsService.ActualizeazaProdusAsync(id, produsActualizat,cancellationToken);
            if(!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }
               

            return Ok(ProdusMapper.ToDto(result.Data!)); 

             
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> StergeProdus(int id, CancellationToken cancellationToken) 
        {
            Result<bool> result = await produsService.StergeProdusAsync(id, cancellationToken);
            if(!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }
            return NoContent(); 

        }

        [HttpGet("test-eroare")]
        public IActionResult TestEroare()
        {
            throw new Exception("Test middleware");
        }
    }
}
