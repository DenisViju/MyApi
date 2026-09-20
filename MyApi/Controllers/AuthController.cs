using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyApi.Common;
using MyApi.Configuration;
using MyApi.DTOs.User;
using MyApi.Services;
using System.Security.Claims;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private const string RefreshTokenCookieName = "refreshToken";

        private readonly IUserService service;
        private readonly IOptions<JwtOptions> jwtOptions;


        public AuthController(IUserService service, IOptions<JwtOptions> jwtOptions)
        {
            this.service = service;
            this.jwtOptions = jwtOptions;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register
            ([FromBody] RegisterDto registerDto, CancellationToken cancellationToken)
        {
            var result = await service.RegisterAsync(registerDto, cancellationToken);

            if(!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return StatusCode(201, result.Data);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login
            ([FromBody] LoginDto loginDto, CancellationToken cancellationToken)
        {
            var result = await service.LoginAsync(loginDto, cancellationToken);
            if (!result.Success)
            {
               return HandleError(result.ErrorType, result.Error);
            }

            SetRefreshTokenCookie(result.Data!.RefreshToken);

            return Ok(result.Data);

        }

        [HttpPut("{id}/reset-password")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> ResetPassword
            (int id, [FromBody] ResetPasswordDto changePasswordDto, CancellationToken cancellationToken)
        {
            Result<UserDto> result = await service.ResetPasswordAsync(id, changePasswordDto.NewPassword, cancellationToken);
            if(!result.Success)
            {
                return  HandleError(result.ErrorType, result.Error); 
            }
            return Ok(result.Data);
            
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<ActionResult<UserDto>> ChangePassword
            ([FromBody] ChangePasswordDto changePasswordDto, CancellationToken cancellationToken)
        {
            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            Result<UserDto> result = await service.ChangePasswordAsync(
                userId,
                changePasswordDto.CurrentPassword,
                changePasswordDto.NewPassword,
                cancellationToken);

            if (!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return Ok(result.Data);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<LoginResponseDto>> Refresh
            (CancellationToken cancellationToken)
        {
            if(!Request.Cookies.TryGetValue(RefreshTokenCookieName, out string? refreshToken)
                || string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized();
            }

            Result<LoginResponseDto> result = await service.
                RefreshTokenAsync(refreshToken, cancellationToken);

            if (!result.Success)
            {
                ClearRefreshTokenCookie();
                return HandleError(result.ErrorType, result.Error);
            }

            SetRefreshTokenCookie(result.Data!.RefreshToken);

            return Ok(result.Data);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout
            (CancellationToken cancellationToken)
        {
            if (Request.Cookies.TryGetValue(RefreshTokenCookieName, out string? refreshToken) &&
               !string.IsNullOrEmpty(refreshToken))
            {
                await service.LogoutAsync(refreshToken, cancellationToken);
            }

            ClearRefreshTokenCookie();

            return NoContent();
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            Response.Cookies.Append(RefreshTokenCookieName, refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpiryDays),
                Path = "/api/Auth"
            });
        }

        private void ClearRefreshTokenCookie()
        {
            Response.Cookies.Delete(RefreshTokenCookieName, new CookieOptions
            {
                Path = "/api/Auth"
            });
        }
    }
}
