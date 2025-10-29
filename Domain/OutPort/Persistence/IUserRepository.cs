
namespace Domain
{
    public interface IUserRepository
    {
        Task<OperationResult> CreateUserAsync(User user, string role);
        Task<User?> FindByUserNameAsync(string userName);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}
