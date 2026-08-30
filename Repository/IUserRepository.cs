using MyApi.Models;

namespace MyApi.Repository
{
    public interface IUserRepository
    {
        Task<bool> ExistaUsernameAsync(string username, CancellationToken cancellationToken);
        Task<User> CreeazaUserAsync(User userNou, CancellationToken cancellationToken);  
    }
}
