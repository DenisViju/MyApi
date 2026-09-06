using Microsoft.Extensions.Options;
using MyApi.Common;
using MyApi.Configuration;
using MyApi.DTOs.User;
using MyApi.Enums;
using MyApi.Mappers;
using MyApi.Models;
using MyApi.Repository;

namespace MyApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordService passwordService;
        private readonly ITokenService tokenService;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IOptions<JwtOptions> options;

        
        public UserService
            (IUserRepository repository, IPasswordService passwordService, 
            ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository,
            IOptions<JwtOptions> options)
        {
            this.userRepository = repository;
            this.passwordService = passwordService;
            this.tokenService = tokenService;
            this.refreshTokenRepository = refreshTokenRepository;
            this.options = options;
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken)
        {
            bool exista = await userRepository.ExistaUsernameAsync(registerDto.Username, cancellationToken);
            if (exista)
            {
                return Result<UserDto>.Fail(
                    "Exista deja un utilizator cu acest username",
                    ResultErrorType.Conflict);
            }
            string passwordHash = passwordService
                .HashPassword(registerDto.Password);

            User user = UserMapper.ToEntity(registerDto, passwordHash);

            User userSalvat = await userRepository.CreeazaUserAsync(user, cancellationToken);

            UserDto userDto = UserMapper.ToDto(userSalvat);

            return Result<UserDto>.Ok(userDto);


        }

        public async Task<Result<LoginResponseDto>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
        {
            User? user = await userRepository.ObtineUserDupaUsernameAsync(loginDto.Username!, cancellationToken);
            if (user == null || !passwordService.VerifyPassword(loginDto.Password!, user.PasswordHash))
            {
                return Result<LoginResponseDto>.Fail(
                    "Username sau parola invalide",
                    ResultErrorType.Unauthorized);
            }

            string accessToken = tokenService.GenereazaJWT(user);
            string refreshToken = tokenService.GenereazaRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(options.Value.RefreshTokenExpiryDays),
                IsRevoked = false,
                UserId = user.Id
            };

            await refreshTokenRepository.SalveazaAsync(refreshTokenEntity, cancellationToken);

            return Result<LoginResponseDto>
                .Ok(Mappers.UserMapper.ToLoginResponseDto(user, accessToken, refreshToken));
            

        }

        public async Task<Result<UserDto>> ResetPasswordAsync(int id, string newPassword, CancellationToken cancellationToken)
        {
            string parolaNouaHash = passwordService.HashPassword(newPassword);
            User? user = await userRepository.ResetareParolaAsync(id, parolaNouaHash, cancellationToken);
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

            User? user = await userRepository.ObtineUserDupaIdAsync(userId, cancellationToken);

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

            await userRepository.SchimbaParolaAsync(user, newPasswordHash, cancellationToken);
  
            return Result<UserDto>.Ok(
                Mappers.UserMapper.ToDto(user));

        }

        public async Task<Result<LoginResponseDto>> RefreshTokenAsync
            (string refreshToken, CancellationToken cancellationToken)
        {
            RefreshToken? refreshTokenEntity = await refreshTokenRepository
                .ObtineDupaTokenAsync(refreshToken, cancellationToken);

            if (refreshTokenEntity == null)
            {
                return Result<LoginResponseDto>.Fail(
                    "Refresh token invalid",
                    ResultErrorType.Unauthorized);
            }

            if (refreshTokenEntity.IsRevoked)
            {
                return Result<LoginResponseDto>.Fail(
                    "Refresh token revocat",
                    ResultErrorType.Unauthorized);
            }

            if (refreshTokenEntity.ExpiresAt <= DateTime.UtcNow)
            {
                return Result<LoginResponseDto>.Fail(
                    "Refresh token expirat",
                    ResultErrorType.Unauthorized);
            }

            User user = refreshTokenEntity.User;

            //de adaugat un transactions pentru revoca si salveaza async
             await refreshTokenRepository
                .RevocaAsync(refreshTokenEntity, cancellationToken);

            string accessToken = tokenService.GenereazaJWT(user);
            string newRefreshToken = tokenService.GenereazaRefreshToken();

            var newRefreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(
                options.Value.RefreshTokenExpiryDays),
                IsRevoked = false,
                UserId = user.Id
            };

            await refreshTokenRepository.SalveazaAsync(newRefreshTokenEntity,cancellationToken);

            return Result<LoginResponseDto>.Ok(Mappers.UserMapper
                .ToLoginResponseDto(user,accessToken, newRefreshToken));
        }

        public async Task<Result<bool>> LogoutAsync(string refreshToken, CancellationToken cancellationToken)
        {
            RefreshToken? refreshTokenEntity = await refreshTokenRepository
                .ObtineDupaTokenAsync(refreshToken, cancellationToken);

            if(refreshTokenEntity ==  null)
            {
                return Result<bool>.Fail(
                    "Refresh token invalid",
                    ResultErrorType.Unauthorized);
            }
            
            if(refreshTokenEntity.IsRevoked)
            {
                return Result<bool>.Fail(
                    "Refresh token deja revocat",
                    ResultErrorType.Unauthorized);
            }

            await refreshTokenRepository.RevocaAsync(refreshTokenEntity, cancellationToken);

            return Result<bool>.Ok(true);
            
        }
    }
}
