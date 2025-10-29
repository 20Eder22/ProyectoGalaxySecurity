namespace Application
{
    public interface ICreateReclamoUseCase
    {
        Task<IdentityResponse> ExecuteAsync(CreateReclamoRequest request);
    }
}