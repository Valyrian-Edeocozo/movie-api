using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using movie_api.ApplicationLayer.Exceptions;

namespace movie_api.ApiLayer.Infrastructure
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> exceptionHandlers;

        public CustomExceptionHandler() =>
            // Register known exception types and handlers.
            this.exceptionHandlers = new()
                {
                { typeof(ValidationException), this.HandleValidationException },
                };

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var exceptionType = exception.GetType();

            if (this.exceptionHandlers.ContainsKey(exceptionType))
            {
                await this.exceptionHandlers[exceptionType].Invoke(httpContext, exception);
                return true;
            }

            return false;
        }

        private async Task HandleValidationException(HttpContext httpContext, Exception ex)
        {
            var exception = (ValidationException)ex;

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails(exception.Errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            });
        }
    }
}
