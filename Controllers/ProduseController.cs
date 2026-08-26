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
        public async  Task<IActionResult> ObtineProduse()
        {
            var produse = await produsService.ObtineToateProdusele();
            
            List<ProdusDto> produseDto = produse
                .Select(p => ProdusMapper.ToDto(p))
                .ToList();

            return Ok(produseDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtineProdus(int id)
        {
            var produs = await produsService.ObtineProdus(id);

            if (produs == null)
                return NotFound();

            ProdusDto produsDto = ProdusMapper.ToDto( produs );

            return Ok(produsDto);
            
            
        }

        [HttpPost]
        public async Task<IActionResult> CreeazaProdus([FromBody] ProdusCreateDto produsCreateDto)
        {
            Categorie? categorie = await produsService.GasesteCategorie(produsCreateDto.CategorieId);
            if(categorie == null)
                return NotFound();

            Produs produs = ProdusMapper.ToEntity(produsCreateDto);
         
            Produs produsSalvat = await produsService.AdaugaProdus(produs);
            ProdusDto produsDto = ProdusMapper.ToDto(produsSalvat);

            return CreatedAtAction(nameof(ObtineProdus), new { id = produsDto.Id },produsDto );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizeazaProdus(int id, [FromBody] ProdusUpdateDto produsUpdateDto)
        {
            Categorie? categorie =
                await produsService.GasesteCategorie(produsUpdateDto.CategorieId);

            if (categorie == null)
                return NotFound();

            Produs produsActualizat = ProdusMapper.ToEntity(produsUpdateDto);
            

            Produs? produsSalvat = await produsService.ActualizeazaProdus(id, produsActualizat);
            if( produsSalvat == null ) 
                return NotFound();

            ProdusDto produsDto = ProdusMapper.ToDto(produsSalvat);
            return Ok( produsDto ); 

             
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> StergeProdus(int id) 
        {
            if(await produsService.StergeProdus(id))
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
