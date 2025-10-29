using Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedAsync(IServiceProvider service)
        {

            var userManager = service.GetRequiredService<UserManager<UserExtension>>();
            var rolManager = service.GetRequiredService<RoleManager<IdentityRole>>();

            var admin = new IdentityRole(RolesConstants.AdminRole);
            var customer = new IdentityRole(RolesConstants.CustomerRole);
            var manager = new IdentityRole(RolesConstants.ManagerRole);

            if (!await rolManager.RoleExistsAsync(RolesConstants.AdminRole)) await rolManager.CreateAsync(admin);
            if (!await rolManager.RoleExistsAsync(RolesConstants.CustomerRole)) await rolManager.CreateAsync(customer);
            if (!await rolManager.RoleExistsAsync(RolesConstants.ManagerRole)) await rolManager.CreateAsync(manager);

            var adminUser = new UserExtension
            {
                FullName = "Eder Arbulu",
                UserName = "admin",
                Email = "eder_arbulu@it-xin.pe",
                EmailConfirmed = true,
                UserId = new Guid("3872DFFD-31BB-478C-A7B2-ED7B2C93BA08")
            };

            var result = await userManager.CreateAsync(adminUser, "4dm1N.2025");
            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, RolesConstants.AdminRole);
            }

            var managerUser = new UserExtension
            {
                FullName = "Daniel Acosta",
                UserName = "dacosta",
                Email = "dacosta@maquimas.pe",
                EmailConfirmed = true,
                UserId = new Guid("B1B6175C-4794-4491-BD48-2E7CD2529CA7")
            };

            var result1 = await userManager.CreateAsync(managerUser, "d4C0st4.2025");
            if (result1.Succeeded)
            {
                await userManager.AddToRoleAsync(managerUser, RolesConstants.ManagerRole);
            }

            var customerUser = new UserExtension
            {
                FullName = "Dany Tello",
                UserName = "dtello",
                Email = "dtello@maquimas.pe",
                EmailConfirmed = true,
                UserId = new Guid("4FD551F6-2813-491D-A096-5ACCEE9E450E")
            };

            var result2 = await userManager.CreateAsync(customerUser, "Password2025");
            if (result2.Succeeded)
            {
                await userManager.AddToRoleAsync(customerUser, RolesConstants.CustomerRole);
            }

        }
    }
}
