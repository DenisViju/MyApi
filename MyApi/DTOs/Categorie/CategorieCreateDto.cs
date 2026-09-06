using System.ComponentModel.DataAnnotations;


namespace MyApi.DTOs.Categorie
{
    public class CategorieCreateDto
    {
        [Required]
        [StringLength(50)]
        public string Nume { get; set; } = string.Empty;

    }
}
