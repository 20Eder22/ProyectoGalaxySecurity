namespace Domain
{
    public interface IEmailApiService
    {
        Task SendEmailAsync(SendEmailDpo request);
    }
}
