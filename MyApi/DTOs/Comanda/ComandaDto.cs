using MyApi.Enums;

namespace MyApi.DTOs.Comanda
{
    public class ComandaDto
    {
        public int Id { get; set; }
        public int UserId { get; set; } 

        public string Strada { get; set; } = string.Empty;
        public string Oras { get; set; } = string.Empty;
        public string Judet { get; set; } = string.Empty;
        public string Tara { get; set; } = string.Empty;
        public string CodPostal { get; set; } = string.Empty;
        public string NumeDestinatar { get; set; } = string.Empty;
        public string TelefonDestinatar { get; set; } = string.Empty;

        public DateTime DataCrearii { get; set; }
        public StatusComanda Status { get; set; } = StatusComanda.Noua;
        public decimal Total { get; set; }

        public List<ElementComandaDto> ElementeComanda { get; set; } = [];

        public byte[] RowVersion { get; set; } = [];
    }
}
