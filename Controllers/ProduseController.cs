using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
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
            return Ok(produse);
        }

        [HttpGet("{id}")]
        public IActionResult ObtineProdus(int id)
        {
            var produs = produsService.ObtineProdus(id);
            if (produs != null)
            {
                return Ok(produs);
            }
            else 
            { 
                return NotFound(); 
            }
        }

        [HttpPost]
        public IActionResult CreeazaProdus([FromBody] Produs produs)
        {
            produsService.AdaugaProdus(produs);
            return CreatedAtAction(nameof(ObtineProdus), new { id = produs.Id },produs );
        }
        [HttpPut("{id}")]
        public IActionResult ActualizeazaProdus(int id, [FromBody] Produs produsActualizat)
        {
            Produs? produs = produsService.ActualizeazaProdus(id, produsActualizat);
            if (produs != null)
            {
                return Ok(produs);
                
            }
            else
            {
                return NotFound();
            }
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
