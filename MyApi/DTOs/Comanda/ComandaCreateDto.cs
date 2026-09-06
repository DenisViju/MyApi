
using System.ComponentModel.DataAnnotations;

namespace MyApi.DTOs.Comanda
{
    public class ComandaCreateDto
    {
        public int AdresaId { get; set; }
        public List<ElementComandaCreateDto> ElementeComandaCreateDto { get; set; } = [];
    }
}
