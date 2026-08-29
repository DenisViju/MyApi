using Microsoft.AspNetCore.Mvc;
using MyApi.Common;

namespace MyApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected ActionResult HandleError(ResultErrorType errorType, string? error)
        {
            return errorType switch
            {
                ResultErrorType.Conflict => Conflict
                    (new ProblemDetails {Status = 409, Title = error }),
                ResultErrorType.NotFound => NotFound
                    (new ProblemDetails { Status = 404, Title = error }),
                _ => BadRequest(new ProblemDetails { Status = 404, Title = error })
            };

            
        }
    }
}
