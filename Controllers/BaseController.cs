using Microsoft.AspNetCore.Mvc;
using MyApi.Common;

namespace MyApi.Controllers
{
    public class BaseController : ControllerBase
    {
        private ProblemDetails CreateProblemDetails(int status, string? error)
        {
            ProblemDetails problemDetails = new ProblemDetails 
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
                ResultErrorType.Conflict => Conflict(CreateProblemDetails(408, error)),
                ResultErrorType.NotFound => NotFound(CreateProblemDetails(404, error)),
                _ => BadRequest(CreateProblemDetails(400, error))
            };

            
        }
    }
}
