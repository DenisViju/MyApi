using MyApi.Enums;

namespace MyApi.DTOs.Comanda
{
    public class ComandaFiltruDto
    {
        public int? UserId { get; set; }
        public StatusComanda? Status { get; set; }
        public DateTime? DataDeLa { get; set; }
        public DateTime? DataPanaLa { get; set; }

        public string? SortBy { get; set; }
        public bool Descending { get; set; }
    }
}