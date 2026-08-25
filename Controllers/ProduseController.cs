using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.DTOs;
using MyApi.Services;

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
            
            List<ProdusDto> produseDto = new List<ProdusDto>();
            foreach (Produs produs in produse )
            {
                ProdusDto produsDto = new ProdusDto
                {
                    Id = produs.Id,
                    Nume = produs.Nume,
                    Pret = produs.Pret
                };
                produseDto.Add( produsDto );
            }
            return Ok(produseDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtineProdus(int id)
        {
            var produs = await produsService.ObtineProdus(id);

            if (produs == null)
                return NotFound();

            ProdusDto produsDto = new ProdusDto
            {
                Id = produs.Id,
                Nume = produs.Nume,
                Pret = produs.Pret
            };
            return Ok(produsDto);
            
            
        }

        [HttpPost]
        public async Task<IActionResult> CreeazaProdus([FromBody] ProdusCreateDto produsCreateDto)
        {
            Produs produs = new Produs
            {
                Nume = produsCreateDto.Nume,
                Pret = produsCreateDto.Pret
            };

            Produs produsSalvat = await produsService.AdaugaProdus(produs);
            ProdusDto produsDto = new ProdusDto
            {
                Id = produsSalvat.Id,
                Nume = produsSalvat.Nume,
                Pret = produsSalvat.Pret
            };

            return CreatedAtAction(nameof(ObtineProdus), new { id = produsDto.Id },produsDto );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizeazaProdus(int id, [FromBody] ProdusUpdateDto produsUpdateDto)
        {
            Produs produsActualizat = new Produs
            {
                Nume = produsUpdateDto.Nume,
                Pret = produsUpdateDto.Pret
            };

            Produs? produsSalvat = await produsService.ActualizeazaProdus(id, produsActualizat);
            if( produsSalvat == null ) 
                return NotFound();

            ProdusDto produsDto = new ProdusDto
            {
                Id = produsSalvat.Id,
                Nume = produsSalvat.Nume,
                Pret = produsSalvat.Pret
            };
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
