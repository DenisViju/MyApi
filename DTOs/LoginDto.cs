using System.ComponentModel.DataAnnotations;

namespace MyApi.DTOs
{
    public class LoginDto
    {
        [Required]
        [StringLength(50)]

        public string? Username { get; set; }

        [Required]
        [MinLength(4)]
        public string? Password { get; set; }
    }
}
