using Microsoft.AspNetCore.Mvc;
using MyApi.Services;
using MyApi.DTOs;

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
    }
}
