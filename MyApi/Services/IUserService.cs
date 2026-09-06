using MyApi.Common;
using MyApi.DTOs.User;
using MyApi.Models;


namespace MyApi.Services
{
    public interface IUserService
    {
        Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);
        Task<Result<LoginResponseDto>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
        Task<Result<UserDto>> ResetPasswordAsync(int id, string newPassword, CancellationToken cancellationToken);
        Task<Result<UserDto>> ChangePasswordAsync
            (int userId, string currentPassword, string newPassword, CancellationToken cancellationToken);
        Task<Result<LoginResponseDto>> RefreshTokenAsync(string refreshToken,CancellationToken cancellationToken);
        Task<Result<bool>> LogoutAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
