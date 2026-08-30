using System.ComponentModel.DataAnnotations;

namespace MyApi.DTOs
{
    public class ProdusUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Nume { get; set; } = string.Empty;
        [Range(0.01, 1000000)]
        public double Pret { get; set; }
        [Range (1, int.MaxValue)]
        public int CategorieId { get; set; }

        public byte[] RowVersion { get; set; } = [];
    }
}
