using Microsoft.AspNetCore.Mvc;
using MyApi.Services;


namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MesajController : ControllerBase
    {
        private readonly IMesajService service;
        public MesajController(IMesajService mesajService)
        {
            service = mesajService;
        }

        [HttpGet("{nume}")]
        public IActionResult GetMesaj(string nume)
        {
            return Ok(service.ObtineMesaj(nume));
        }

        
        [HttpGet("personalizat")]
        public IActionResult GetMesajQuery([FromQuery] string nume)
        {
            return Ok(service.ObtineMesaj(nume));
        }
       
    }
}
