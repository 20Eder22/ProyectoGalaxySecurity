using Domain;

namespace Application
{
    public class RefreshTokenUseCase(IAuthService authService) : IRefreshTokenUseCase
    {
        public async Task<string> ExecuteAsync()
        {
            var result = await authService.RefreshTokensAsync();
            return result.AccessToken;
        }
    }
}
