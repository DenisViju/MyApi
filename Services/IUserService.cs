using MyApi.Common;
using MyApi.DTOs;
using MyApi.Models;


namespace MyApi.Services
{
    public interface IUserService
    {
        Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);
        Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
    }
}
