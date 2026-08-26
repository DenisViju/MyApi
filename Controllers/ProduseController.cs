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
                    Pret = produs.Pret,
                    CategorieId = produs.CategorieId,
                    NumeCategorie = produs.Categorie?.Nume
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
                Pret = produs.Pret,
                CategorieId = produs.CategorieId,
                NumeCategorie = produs.Categorie?.Nume
            };
            return Ok(produsDto);
            
            
        }

        [HttpPost]
        public async Task<IActionResult> CreeazaProdus([FromBody] ProdusCreateDto produsCreateDto)
        {
            Categorie? categorie = await produsService.GasesteCategorie(produsCreateDto.CategorieId);
            if(categorie == null)
                return NotFound();

            Produs produs = new Produs
            {
                Nume = produsCreateDto.Nume,
                Pret = produsCreateDto.Pret,
                CategorieId = produsCreateDto.CategorieId,
                

            };


            Produs produsSalvat = await produsService.AdaugaProdus(produs);
            ProdusDto produsDto = new ProdusDto
            {
                Id = produsSalvat.Id,
                Nume = produsSalvat.Nume,
                Pret = produsSalvat.Pret,
                CategorieId = produsSalvat.CategorieId,
                NumeCategorie = categorie.Nume
            };

            return CreatedAtAction(nameof(ObtineProdus), new { id = produsDto.Id },produsDto );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizeazaProdus(int id, [FromBody] ProdusUpdateDto produsUpdateDto)
        {
            Categorie? categorie =
                await produsService.GasesteCategorie(produsUpdateDto.CategorieId);

            if (categorie == null)
                return NotFound();

            Produs produsActualizat = new Produs
            {
                Nume = produsUpdateDto.Nume,
                Pret = produsUpdateDto.Pret,
                CategorieId = produsUpdateDto.CategorieId
            };
            

            Produs? produsSalvat = await produsService.ActualizeazaProdus(id, produsActualizat);
            if( produsSalvat == null ) 
                return NotFound();

            ProdusDto produsDto = new ProdusDto
            {
                Id = produsSalvat.Id,
                Nume = produsSalvat.Nume,
                Pret = produsSalvat.Pret,
                CategorieId = produsSalvat.CategorieId,
                NumeCategorie = categorie.Nume

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
