namespace Domain
{
    public interface IAuthService
    {
        Task<(string AccessToken, string RefreshToken)> GenerateTokenAsync(User userApp);
        Task<(string AccessToken, string RefreshToken, User? User)> RefreshTokensAsync();
        void RemoveAuthCookies();
    }
}
