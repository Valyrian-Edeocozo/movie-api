using movie_api.ApplicationLayer.Common.ResponseModels.View;
using movie_api.ApplicationLayer.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace movie_api.ApiLayer.Infrastructure
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestTime = DateTime.Now;
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, requestTime);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, DateTime requestTime)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch (exception)
            {
                case ValidationException:
                    code = HttpStatusCode.OK;
                    break;
                case BadRequestException:
                    break;
            }

            _logger.LogError(exception.Message, exception);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            if (exception is ValidationException ex)
            {
                result = JsonConvert.SerializeObject(Result<dynamic>.Failure(requestTime, exception.Message, ex.Errors));
            }
            if (string.IsNullOrEmpty(result))
            {
                result = JsonConvert.SerializeObject(Result<dynamic>.Failure(requestTime, exception.Message));
            }

            return context.Response.WriteAsync(result);
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
