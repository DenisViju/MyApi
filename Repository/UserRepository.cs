
using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.Models;

namespace MyApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AplicatieDbContext context;
        public UserRepository(AplicatieDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> ExistaUsernameAsync(string username, CancellationToken cancellationToken)
        {
            return await context.Users
                .AnyAsync(u => u.Username == username, cancellationToken);
        }

        public async Task<User> CreeazaUserAsync(User user, CancellationToken cancellationToken)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);
            
            return user;
        }

        public async Task<User?> ObtineUserDupaUsernameAsync(string username, CancellationToken cancellationToken)
        {
            return await context.Users
                .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        }

        public async Task<User?> ResetareParolaAsync(int id, string parolaNouaHash, CancellationToken cancellationToken)
        {
            User? user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user == null)
            {
                return null;
            }

            user.PasswordHash = parolaNouaHash;
            await context.SaveChangesAsync(cancellationToken);

            return user;
        }

        public async Task<User?> ObtineUserDupaIdAsync(int id, CancellationToken cancellationToken)
        {
            return await context.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task SchimbaParolaAsync(User user, string parolaNouaHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = parolaNouaHash;
            await context.SaveChangesAsync(cancellationToken);
        }


    }
}
