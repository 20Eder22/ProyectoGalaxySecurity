using Application;
using Domain;
using System.Net;
using System.Text.Json;

namespace API
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (DomainException ex)
            {
                logger.LogWarning(ex, $"Domain exception ocurred: {ex.Message}");
                await HandleExceptionAsync(context, ex.Message, "DOMAIN_ERROR", HttpStatusCode.BadRequest);
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogError(ex, $"Unauthorized request: {ex.Message}");
                await HandleExceptionAsync(context, ex.Message, "Unauthorized", HttpStatusCode.Unauthorized);
            }
            catch (ApplicationException ex)
            {
                logger.LogError(ex, $"Application exception ocurred: {ex.Message}");
                await HandleExceptionAsync(context, ex.Message, "APP_ERROR", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Unexpected error ocurred: {ex.Message}");
                await HandleExceptionAsync(context, ex.Message, "SERVER_ERROR", HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, string message, string errorCode, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = BaseResponse<string>.Failure(message, errorCode);

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            await context.Response.WriteAsync(json);
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
