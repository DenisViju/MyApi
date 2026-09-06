namespace MyApi.DTOs.Produs
{
    public class ProdusFiltruDto
    {
        public int? CategorieId { get; set; }
        public decimal? PretMinim { get; set; }
        public decimal? PretMaxim { get; set; }
        public string? Nume { get; set; }

        public string? SortBy { get; set; }
        public bool Descending { get; set; }
    }
}
