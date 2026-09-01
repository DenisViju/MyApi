using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.Models;

namespace MyApi.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AplicatieDbContext context;

        public RefreshTokenRepository(AplicatieDbContext context)
        {
            this.context = context;
        }

        public async Task<RefreshToken?> ObtineDupaTokenAsync(string token, CancellationToken cancellationToken)
        {
            return await context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt =>  rt.Token == token, cancellationToken);

        }

        public async Task SalveazaAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task RevocaAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            refreshToken.IsRevoked = true;
            await context.SaveChangesAsync(cancellationToken);  
        }
    }
}
