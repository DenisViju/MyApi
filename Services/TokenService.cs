using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyApi.Configuration;
using MyApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<JwtOptions> options;
        public TokenService(IOptions<JwtOptions> options)
        {
            this.options = options;
        }
        
        public string GenereazaJWT (User user)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey
                (System.Text.Encoding.UTF8.GetBytes(options.Value.Key));

            var signingCredentials = new SigningCredentials
                (symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, user.Role)

            };

            var token = new JwtSecurityToken(
                issuer: options.Value.Issuer,
                audience: options.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(options.Value.ExpiryHours),
                signingCredentials: signingCredentials);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }
    }
}
