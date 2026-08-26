using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriiController : ControllerBase
    {
        private readonly AplicatieDbContext context;
        public CategoriiController(AplicatieDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObtineCategorii()
        {
            List<Categorie> categorii = await context.Categorii.ToListAsync();

            return Ok(categorii);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtineCategorie(int id)
        {
            Categorie? categorie = await context.Categorii.FirstOrDefaultAsync(c => c.Id == id);
            if (categorie == null)
            {
                return NotFound();
            }
            return Ok(categorie);

        }

        [HttpPost]
        public async Task<IActionResult> CreeazaCategorie([FromBody] Categorie categorieNoua)
        {
            await context.Categorii.AddAsync(categorieNoua);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObtineCategorie), new { id = categorieNoua.Id }, categorieNoua);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizeazaCategorie(int id, [FromBody] Categorie categorieActualizata)
        {
            Categorie? categorie = await context.Categorii.FirstOrDefaultAsync(c => c.Id == id);
            if (categorie == null)
            {
                return NotFound();
            }
            categorie.Nume = categorieActualizata.Nume;
            await context.SaveChangesAsync();
            return Ok(categorie);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> StergeCategorie(int id)
        {
            Categorie? categorie = await context.Categorii.FirstOrDefaultAsync(c => c.Id == id);
            if (categorie == null)
            {
                return NotFound();
            }
            context.Categorii.Remove(categorie);
            await context.SaveChangesAsync();   
            return NoContent();
        }

    }
}
