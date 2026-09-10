using System.ComponentModel.DataAnnotations;

namespace MyApi.DTOs.Produs
{
    public class ProdusCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Nume { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        public string Descriere { get; set; } = string.Empty;
        [Range(0.01, 1000000)]
        public decimal Pret {  get; set; }

        [Range(1, int.MaxValue)]
        public int Stoc {  get; set; }
        [Range(1, int.MaxValue)]
        public int CategorieId { get; set; }

              
    }
}
