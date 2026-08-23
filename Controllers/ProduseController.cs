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
        public IActionResult ObtineProduse()
        {
            var produse = produsService.ObtineToateProdusele();
            
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
        public IActionResult ObtineProdus(int id)
        {
            var produs = produsService.ObtineProdus(id);

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
        public IActionResult CreeazaProdus([FromBody] ProdusCreateDto produsCreateDto)
        {
            Produs produs = new Produs
            {
                Nume = produsCreateDto.Nume,
                Pret = produsCreateDto.Pret
            };

            Produs produsSalvat = produsService.AdaugaProdus(produs);
            ProdusDto produsDto = new ProdusDto
            {
                Id = produsSalvat.Id,
                Nume = produsSalvat.Nume,
                Pret = produsSalvat.Pret
            };

            return CreatedAtAction(nameof(ObtineProdus), new { id = produsDto.Id },produsDto );
        }

        [HttpPut("{id}")]
        public IActionResult ActualizeazaProdus(int id, [FromBody] ProdusUpdateDto produsUpdateDto)
        {
            Produs produsActualizat = new Produs
            {
                Nume = produsUpdateDto.Nume,
                Pret = produsUpdateDto.Pret
            };

            Produs? produsSalvat = produsService.ActualizeazaProdus(id, produsActualizat);
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
        public IActionResult StergeProdus(int id) 
        {
            if(produsService.StergeProdus(id))
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
