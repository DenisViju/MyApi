namespace MyApi.DTOs.User
{
    public class LoginResponseDto
    {
        public UserDto? UserDto { get; set; }
        public string? Token {  get; set; }
    }
}
