using MyApi.Models;
namespace MyApi.Repository
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> ObtineDupaTokenAsync(string token, CancellationToken cancellationToken);
        Task SalveazaAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
        Task RevocaAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    }
}
