using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Infraestructure
{
    public static class VaultConfigurationExtensions
    {
        public static IServiceCollection AddVaultSecrets(this IServiceCollection services, IConfiguration configuration)
        {
            var vaultConfig = configuration.GetSection("Vault");
            var vaultAddress = vaultConfig["Address"];
            var vaultToken = vaultConfig["Token"];
            var secretsPath = vaultConfig["SecretsPath"];
            var mountPoint = vaultConfig["MountPoint"];

            // crear cliente de Vault
            var authMethod = new TokenAuthMethodInfo(vaultToken);
            var vaultClientSettings = new VaultClientSettings(vaultAddress, authMethod);
            IVaultClient vaultClient = new VaultClient(vaultClientSettings);

            // puedes ponerlo como singleton
            services.AddSingleton(vaultClient);

            // Registrar un servicio que obtenga secretos cuando se necesite
            services.AddScoped<IVaultSecretsProvider>(sp =>
                new VaultSecretsProvider(vaultClient, secretsPath, mountPoint));

            return services;
        }
    }
}
