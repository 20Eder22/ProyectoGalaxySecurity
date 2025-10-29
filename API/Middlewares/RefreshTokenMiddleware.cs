using Application;

namespace API
{
    public class RefreshTokenMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            using var scope = serviceProvider.CreateScope();
            var refreshUseCase = scope.ServiceProvider.GetRequiredService<IRefreshTokenUseCase>();

            if (
                context.Request.Path.StartsWithSegments("/api/auth/login") ||
                context.Request.Path.StartsWithSegments("/api/auth/logout") ||
                context.Request.Path.StartsWithSegments("/api/auth/refresh"))
            {
                await next(context);
                return;
            }

            var accessToken = context.Request.Cookies["access_token"];
            var refreshToken = context.Request.Cookies["refresh_token"];

            if (!string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                await next(context);
                return;
            }

            accessToken = await refreshUseCase.ExecuteAsync();
            context.Request.Headers["Authorization"] = $"Bearer {accessToken}";
            await next(context);
        }
    }
}
