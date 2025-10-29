namespace Domain
{
    public interface IVaultSecretsProvider
    {
        Task<Dictionary<string, string>> GetSecretsAsync();
        Task<string?> GetSecretAsync(string key);
    }
}
