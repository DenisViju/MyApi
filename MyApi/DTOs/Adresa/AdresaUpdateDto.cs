using System.ComponentModel.DataAnnotations;

namespace MyApi.DTOs.Adresa
{
    public class AdresaUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string NumeDestinatar { get; set; } = string.Empty;
        [Required]
        [StringLength(150)]
        public string Strada { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string Oras { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string Judet { get; set; } = string.Empty;
        [Required]
        [StringLength(10)]
        public string CodPostal { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string Tara { get; set; } = string.Empty;
        [Required]
        [StringLength(20)]
        public string NumarTelefon { get; set; } = string.Empty;
        public bool EstePrincipala { get; set; }
    }
}
