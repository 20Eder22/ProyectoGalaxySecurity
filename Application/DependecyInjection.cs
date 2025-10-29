using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
            services.AddScoped<ILoginUseCase, LoginUseCase>();
            services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();
            services.AddScoped<IRemoveCookiesUseCase, RemoveCookiesUseCase>();
            services.AddScoped<ICreateReclamoUseCase, CreateReclamoUseCase>();
            services.AddScoped<IGetReclamoByCodeUseCase, GetReclamoByCodeUseCase>();
            services.AddScoped<IGetReclamosByDateUseCase, GetReclamosByDateUseCase>();
            services.AddScoped<IVerifyCaptchaUseCase, VerifyCaptchaUseCase>();
            services.AddScoped<IRedeemUseCase, RedeemUseCase>();
            services.AddScoped<ICreateChallengeUseCase, CreateChallengeUseCase>();
            return services;
        }
    }
}
