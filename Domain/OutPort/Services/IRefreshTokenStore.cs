namespace Domain
{
    public interface IRefreshTokenStore
    {
        Task SaveTokenAsync(string userId, string refreshToken, TimeSpan expiration);
        Task<RefreshTokenDpo?> GetTokenAsync(string refreshToken);
        Task InvalidateTokenAsync(string refreshToken);
        Task<string> GetUserIdFromTokenAsync(string refreshToken);
    }
}
