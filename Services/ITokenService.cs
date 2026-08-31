using MyApi.Models;


namespace MyApi.Services
{
    public interface ITokenService
    {
        string GenereazaJWT(User user);
    }
}
