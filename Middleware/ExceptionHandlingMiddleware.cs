using Microsoft.AspNetCore.Mvc;



namespace MyApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;
        private readonly IProblemDetailsService problemDetailsService;

        public ExceptionHandlingMiddleware
            (RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IProblemDetailsService problemDetailsService)
        {
            this.next = next;
            this.logger = logger;
            this.problemDetailsService = problemDetailsService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "A aparut o eroare");
                await HandleExceptionAsync(context);
            }
        }
        public async Task HandleExceptionAsync(HttpContext context)
        {
            
            var problemDetails = new ProblemDetails
            {
                Status = 500,
                Title = "A aparut o eroare interna."
            };
            ProblemDetailsContext problemDetailsContext = new ProblemDetailsContext 
            { 
                HttpContext = context,
                ProblemDetails = problemDetails,
            };

            await problemDetailsService.WriteAsync(problemDetailsContext);

            

        }

    }
}
