using MyApi.DTOs;
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
