
namespace Application
{
    public interface ICreateUserUseCase
    {
        Task<IdentityResponse> ExecuteAsync(CreateUserRequest request);
    }
}
