using MyApi.Common;
using MyApi.DTOs.User;
using MyApi.Mappers;
using MyApi.Models;
using MyApi.Repository;

namespace MyApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        private readonly IPasswordService passwordService;
        private readonly ITokenService tokenService;
        public UserService(IUserRepository repository, IPasswordService passwordService, ITokenService tokenService)
        {
            this.repository = repository;
            this.passwordService = passwordService;
            this.tokenService = tokenService;
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken)
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

        public async Task<Result<LoginResponseDto>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
        {
            User? user = await repository.ObtineUserDupaUsernameAsync(loginDto.Username!, cancellationToken);
            if (user == null || !passwordService.VerifyPassword(loginDto.Password!, user.PasswordHash))
            {
                return Result<LoginResponseDto>.Fail(
                    "Username sau parola invalide",
                    ResultErrorType.Unauthorized);
            }

            return Result<LoginResponseDto>
                .Ok(Mappers.UserMapper.ToLoginResponseDto(user, tokenService.GenereazaJWT(user)));
            

        }

        public async Task<Result<UserDto>> ResetPasswordAsync(int id, string newPassword, CancellationToken cancellationToken)
        {
            string parolaNouaHash = passwordService.HashPassword(newPassword);
            User? user = await repository.ResetareParolaAsync(id, parolaNouaHash, cancellationToken);
            if(user == null)
            {
                return Result<UserDto>.Fail (
                    "Utilizatorul nu exista",
                    ResultErrorType.NotFound);
            }

            
            return Result<UserDto>.Ok(Mappers.UserMapper.ToDto(user));

        }
        public async Task<Result<UserDto>> ChangePasswordAsync
            (int userId, string currentPassword, string newPassword, CancellationToken cancellationToken)
        {

            User? user = await repository.ObtineUserDupaIdAsync(userId, cancellationToken);

            if (user == null)
            {
                return Result<UserDto>.Fail(
                    "Utilizatorul nu exista",
                    ResultErrorType.NotFound);
            }

            bool parolaCorecta = passwordService.VerifyPassword(currentPassword, user.PasswordHash);

            if (!parolaCorecta)
            {
                return Result<UserDto>.Fail(
                    "Parola curenta este incorecta",
                    ResultErrorType.Unauthorized);
            }
            string newPasswordHash = passwordService.HashPassword(newPassword);

            await repository.SchimbaParolaAsync(user, newPasswordHash, cancellationToken);
  
            return Result<UserDto>.Ok(
                Mappers.UserMapper.ToDto(user));

        }
    }
}
