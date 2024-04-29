using Microsoft.AspNetCore.Diagnostics;

namespace MailService.Middlewares
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;
        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, exception.Message, exception?.InnerException?.Message);
            await HandleExceptionAsync(httpContext, exception!);
            return true;
        }

        private async Task HandleExceptionAsync(HttpContext context , Exception exception)
        {
            var error = new
            {
                Code = "ApiUnhandledException",
                exception.Message,
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(error);
        }
    }
}
