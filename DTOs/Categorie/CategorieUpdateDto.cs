using System.ComponentModel.DataAnnotations;

namespace MyApi.DTOs.Categorie
{
    public class CategorieUpdateDto
    {
        [Required]
        [StringLength(50)]
        public string Nume { get; set; } = string.Empty;
        public byte[] RowVersion { get; set; } = [];
    }
}
