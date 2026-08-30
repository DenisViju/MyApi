namespace MyApi.DTOs
{
    public class ProdusFiltruDto
    {
        public int? CategorieId { get; set; }
        public double? PretMinim { get; set; }
        public double? PretMaxim { get; set; }
        public string? Nume { get; set; }

        public string? SortBy { get; set; }
        public bool Descending { get; set; }
    }
}
