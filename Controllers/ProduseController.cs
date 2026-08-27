using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.DTOs;
using MyApi.Services;
using MyApi.Mappers;
using MyApi.Common;


namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProduseController : ControllerBase
    {
        private readonly IProdusService produsService;
        public ProduseController(IProdusService produsService)
        {
            this.produsService = produsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProdusDto>>> ObtineProduse()
        {
            var produse = await produsService.ObtineToateProduseleAsync();
            
            List<ProdusDto> produseDto = produse
                .Select(p => ProdusMapper.ToDto(p))
                .ToList();

            return Ok(produseDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdusDto>> ObtineProdus(int id)
        {
            var result = await produsService.ObtineProdusAsync(id);

            if(!result.Success)
            {
                return result.ErrorType switch
                {
                    ResultErrorType.NotFound => NotFound(result.Error),
                    _ => BadRequest(result.Error)

                };
            }

            return Ok( ProdusMapper.ToDto(result.Data!));

            
            
        }

        [HttpPost]
        public async Task<ActionResult<ProdusDto>> CreeazaProdus([FromBody] ProdusCreateDto produsCreateDto)
        {

            Produs produs = ProdusMapper.ToEntity(produsCreateDto);
         
            Result<Produs> result = 
                await produsService.AdaugaProdusAsync(produs);

            if(!result.Success)
            {
                return result.ErrorType switch
                {
                    ResultErrorType.Conflict => Conflict(result.Error),
                    ResultErrorType.NotFound => NotFound(result.Error),
                    _ => BadRequest(result.Error)
                };
            }
            
            
            ProdusDto produsDto = 
                ProdusMapper.ToDto(result.Data!);

            return CreatedAtAction(nameof(ObtineProdus), new { id = produsDto.Id },produsDto );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProdusDto>> ActualizeazaProdus(int id, [FromBody] ProdusUpdateDto produsUpdateDto)
        {
           
            Produs produsActualizat = ProdusMapper.ToEntity(produsUpdateDto);

            Result<Produs> result = await produsService.ActualizeazaProdusAsync(id, produsActualizat);
            if(!result.Success)
            {
                return result.ErrorType switch
                {
                    ResultErrorType.NotFound => NotFound(result.Error),
                    ResultErrorType.Conflict => Conflict(result.Error),
                    _ => BadRequest(result.Error)
                };
            }
               

            return Ok(ProdusMapper.ToDto(result.Data!)); 

             
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> StergeProdus(int id) 
        {
            Result<bool> result = await produsService.StergeProdusAsync(id);
            if(!result.Success)
            {
                return result.ErrorType switch
                {
                    ResultErrorType.NotFound => NotFound(result.Error),
                    _ => BadRequest(result.Error)
                };
            }
            return NoContent(); 

        }
    }
}
