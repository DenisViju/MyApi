using MyApi.Common;
using MyApi.DTOs;
using MyApi.Mappers;
using MyApi.Models;
using MyApi.Repository;

namespace MyApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        private readonly IPasswordService passwordService;
        public UserService(IUserRepository repository, IPasswordService passwordService)
        {
            this.repository = repository;
            this.passwordService = passwordService;
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto,  CancellationToken cancellationToken)
        {
            bool exista = await repository.ExistaUsernameAsync(registerDto.Username, cancellationToken);
            if (exista)
            {
                return Result<UserDto>.Fail(
                    "Exista deja un utilizator cu acest username",
                    ResultErrorType.Conflict);
            }
            string passwordHash = passwordService
                .HashPassword(registerDto.Password);

            User user = UserMapper.ToEntity(registerDto, passwordHash);

            User userSalvat = await repository.CreeazaUserAsync(user, cancellationToken);

            UserDto userDto = UserMapper.ToDto(userSalvat);

            return Result<UserDto>.Ok(userDto);
                
               
        }
    }
}
