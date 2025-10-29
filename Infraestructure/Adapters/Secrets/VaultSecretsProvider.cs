using Domain;
using VaultSharp;

namespace Infraestructure
{
    public class VaultSecretsProvider(IVaultClient vaultClient, string secretsPath, string mountPoint)
        : IVaultSecretsProvider
    {
        public async Task<Dictionary<string, string>> GetSecretsAsync()
        {
            var secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
                path: secretsPath,
                mountPoint: mountPoint
            );

            var data = secret.Data.Data;
            var dict = new Dictionary<string, string>();

            foreach (var kv in data)
            {
                dict[kv.Key] = kv.Value?.ToString() ?? "";
            }

            return dict;
        }

        public async Task<string?> GetSecretAsync(string key)
        {
            var all = await GetSecretsAsync();
            all.TryGetValue(key, out var val);
            return val;
        }
    }
}
