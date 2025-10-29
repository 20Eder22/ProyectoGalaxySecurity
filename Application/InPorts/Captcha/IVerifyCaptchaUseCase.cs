namespace Application
{
    public interface IVerifyCaptchaUseCase
    {
        Task<(bool IsValid, string Message)> ExecuteAsync(string token);
    }
}
