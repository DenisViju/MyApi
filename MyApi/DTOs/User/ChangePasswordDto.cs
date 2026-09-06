using System.ComponentModel.DataAnnotations;

namespace MyApi.DTOs.User
{
    public class ChangePasswordDto
    {
        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string CurrentPassword { get; set; } = string.Empty;
        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string NewPassword { get; set; } = string.Empty;

    }
}
