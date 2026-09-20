using System.Text.Json.Serialization;

namespace MyApi.DTOs.User
{
    public class LoginResponseDto
    {
        public UserDto UserDto { get; set; } = null!;
        public string AccessToken { get; set; } = string.Empty;
        [JsonIgnore]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
