using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApi.Common;
using MyApi.DTOs.User;
using MyApi.Services;
using System.Security.Claims;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IUserService service;

        public AuthController(IUserService service)
        {
            this.service = service;
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
            ([FromBody] RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken)
        {
            Result<LoginResponseDto> result = await service.
                RefreshTokenAsync(refreshTokenDto.RefreshToken, cancellationToken);

            if (!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return Ok(result.Data);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout
            ([FromBody] RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken)
        {
            Result<bool> result = await service.
                LogoutAsync(refreshTokenDto.RefreshToken, cancellationToken);

            if(!result.Success)
            {
                return HandleError(result.ErrorType, result.Error);
            }

            return NoContent();
        }

    }
}
