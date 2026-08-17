using Microsoft.AspNetCore.Diagnostics;

namespace ECommerce.API.MiddleWares
{
    public class GlobalExceptionMiddleware(IProblemDetailsService problemDetailsService,ILogger logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "UnHandled Exception");
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var problemDetailsContext = new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal Server Error",
                    Detail = "An Unexpected error occurred . Please try again later."
                }
            };
            return await problemDetailsService.TryWriteAsync(problemDetailsContext);
        }

    }
}
