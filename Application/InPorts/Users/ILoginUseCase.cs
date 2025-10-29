namespace Application
{
    public interface ILoginUseCase
    {
        Task<string> ExecuteAsync(LoginRequest request);
    }
}
