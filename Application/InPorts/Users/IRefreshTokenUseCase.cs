namespace Application
{
    public interface IRefreshTokenUseCase
    {
        Task<string> ExecuteAsync();
    }
}
