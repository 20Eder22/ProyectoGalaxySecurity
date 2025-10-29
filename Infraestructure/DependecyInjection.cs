using Domain;
using Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infraestructure
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {

            // Integrar Vault
            services.AddVaultSecrets(configuration);

            var sp = services.BuildServiceProvider();
            var secretProvider = sp.GetRequiredService<IVaultSecretsProvider>();
            var secrets = secretProvider.GetSecretsAsync().GetAwaiter().GetResult(); // sincronizar

            // Inyección del contexto de base de datos
            services.AddDbContext<IdentityDbContext>((sp, options) =>
            {
                var cs = secrets["DbSecurity"];
                options.UseNpgsql(cs);
            });

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisCon = secrets["RedisConnection"];
                return ConnectionMultiplexer.Connect(redisCon);
            });

            //ASPNET Core Identity Injection
            services.AddIdentity<UserExtension, IdentityRole>(policy =>
            {
                policy.Password.RequiredLength = SecurityConstants.MinLengthPassword;
                policy.Password.RequireDigit = true;
                policy.Password.RequireLowercase = true;
                policy.Password.RequireUppercase = true;
                policy.Password.RequireNonAlphanumeric = false;
                policy.User.RequireUniqueEmail = true;
                policy.SignIn.RequireConfirmedEmail = true;
                policy.Lockout.MaxFailedAccessAttempts = SecurityConstants.MaxFailedAccessAttempts;
                policy.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(SecurityConstants.LockoutTimeSpan);
                policy.Lockout.AllowedForNewUsers = true;
            })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();


            //Configuración del servicio de Email

            services.AddHttpClient<IEmailApiService, EmailApiService>((sp, cliente) =>
            {
                var apiPath = secrets["APIEmailPath"];
                cliente.BaseAddress = new Uri(apiPath);
                Console.WriteLine(apiPath);
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IReclamoRepository, ReclamoRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRefreshTokenStore, RefreshTokenStore>();
            services.AddScoped<ICaptchaService, CaptchaService>();
            return services;
        }
    }
}
