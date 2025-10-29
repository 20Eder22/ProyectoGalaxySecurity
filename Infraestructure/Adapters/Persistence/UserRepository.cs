using Domain;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace Infraestructure
{
    public class UserRepository(UserManager<UserExtension> userManager) : IUserRepository
    {
        public async Task<OperationResult> CreateUserAsync(User user, string role)
        {
            var UserExtension = user.Adapt<UserExtension>();
            var result = await userManager.CreateAsync(UserExtension, user.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(UserExtension, role);
            }

            return result.Succeeded
                ? OperationResult.Ok()
                : OperationResult.Fail(result.Errors.Select(e => e.Description));
        }


        public async Task<User?> FindByUserNameAsync(string userName)
        {
            var result = await userManager.FindByNameAsync(userName);
            return result.Adapt<User>();
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var userExt = user.Adapt<UserExtension>();
            var result = await userManager.CheckPasswordAsync(userExt, password);
            return result;
        }
    }
}
