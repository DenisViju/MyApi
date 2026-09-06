using Microsoft.AspNetCore.Identity;

namespace MyApi.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<object> passwordHasher = new();

        public string HashPassword(string password)
        {
            return passwordHasher.HashPassword(null!, password); 
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            PasswordVerificationResult result =
                passwordHasher.VerifyHashedPassword(
                    null!,
                    passwordHash,
                    password);

            return result == PasswordVerificationResult.Success;
        }

    }
}
