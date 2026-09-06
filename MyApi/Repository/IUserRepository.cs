using MyApi.Models;

namespace MyApi.Repository
{
    public interface IUserRepository
    {
        Task<bool> ExistaUsernameAsync(string username, CancellationToken cancellationToken);
        Task<User> CreeazaUserAsync(User userNou, CancellationToken cancellationToken);  
        Task<User?> ObtineUserDupaUsernameAsync(string username, CancellationToken cancellationToken);
        Task<User?> ResetareParolaAsync(int id, string parolaNoua, CancellationToken cancellationToken);
        Task<User?> ObtineUserDupaIdAsync(int id, CancellationToken cancellationToken);
        Task SchimbaParolaAsync(User user, string parolaNouaHash, CancellationToken cancellationToken);
    }
}
