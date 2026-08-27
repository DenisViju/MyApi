using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.DTOs;
using MyApi.Services;
using MyApi.Mappers;

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
            var produs = await produsService.ObtineProdusAsync(id);

            if (produs == null)
                return NotFound();

            ProdusDto produsDto = ProdusMapper.ToDto( produs );

            return Ok(produsDto);
            
            
        }

        [HttpPost]
        public async Task<ActionResult<ProdusDto>> CreeazaProdus([FromBody] ProdusCreateDto produsCreateDto)
        {
            Categorie? categorie = await produsService.GasesteCategorieAsync(produsCreateDto.CategorieId);
            if(categorie == null)
                return NotFound();

            Produs produs = ProdusMapper.ToEntity(produsCreateDto);
         
            Produs produsSalvat = await produsService.AdaugaProdusAsync(produs);
            ProdusDto produsDto = ProdusMapper.ToDto(produsSalvat);

            return CreatedAtAction(nameof(ObtineProdus), new { id = produsDto.Id },produsDto );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProdusDto>> ActualizeazaProdus(int id, [FromBody] ProdusUpdateDto produsUpdateDto)
        {
            Categorie? categorie =
                await produsService.GasesteCategorieAsync(produsUpdateDto.CategorieId);

            if (categorie == null)
                return NotFound();

            Produs produsActualizat = ProdusMapper.ToEntity(produsUpdateDto);
            

            Produs? produsSalvat = await produsService.ActualizeazaProdusAsync(id, produsActualizat);
            if( produsSalvat == null ) 
                return NotFound();

            ProdusDto produsDto = ProdusMapper.ToDto(produsSalvat);
            return Ok( produsDto ); 

             
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> StergeProdus(int id) 
        {
            if(await produsService.StergeProdusAsync(id))
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }

        }
    }
}
