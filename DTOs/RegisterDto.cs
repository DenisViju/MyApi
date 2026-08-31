using System.ComponentModel.DataAnnotations;

namespace MyApi.DTOs
{
    public class RegisterDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string Password { get; set; } = string.Empty;
    }
}
