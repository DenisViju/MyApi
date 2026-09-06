using MyApi.DTOs.User;
using MyApi.Models;
using MyApi.Services;

namespace MyApi.Mappers
{
    public static class UserMapper
    {

        public static UserDto ToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            };
        }
        public static LoginResponseDto ToLoginResponseDto (User user, string accesToken, string refreshToken)
        {
            return new LoginResponseDto
            {
                UserDto = ToDto(user),
                AccessToken = accesToken,
                RefreshToken = refreshToken
            };
        }
        public static User ToEntity(RegisterDto registerDto, string passwordHash )
        {
         

            return new User
            {
                Username = registerDto.Username,
                PasswordHash = passwordHash,
                Role = "User"
            };
        }
    }
}
