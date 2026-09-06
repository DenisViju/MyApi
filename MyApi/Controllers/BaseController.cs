using Microsoft.AspNetCore.Mvc;
using MyApi.Enums;
using System.Security.Claims;

namespace MyApi.Controllers
{
    public class BaseController : ControllerBase
    {
        private ProblemDetails CreateProblemDetails(int status, string? error)
        {
            var problemDetails = new ProblemDetails 
            {
                Type = $"https://httpstatuses.com/{status}",
                Title = error,
                Status = status,
                Instance = HttpContext.Request.Path
            };
            problemDetails.Extensions["traceId"] = HttpContext.TraceIdentifier;

            return problemDetails;
        }


        protected ActionResult HandleError(ResultErrorType errorType, string? error)
        {

            return errorType switch
            {
                ResultErrorType.Conflict => Conflict(CreateProblemDetails(409, error)),
                ResultErrorType.NotFound => NotFound(CreateProblemDetails(404, error)),
                ResultErrorType.Unauthorized => Unauthorized(CreateProblemDetails(401, error)),
                _ => BadRequest(CreateProblemDetails(400, error))
            };

            
        }
        protected int ObtineUserId()
        {
            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return -1;
            }
            return userId;
        }
    }
}
