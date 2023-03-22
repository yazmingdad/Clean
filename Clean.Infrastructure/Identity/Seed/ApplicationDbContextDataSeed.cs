using Clean.Core.Constants;
using Clean.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Clean.Infrastructure.Identity.Seed
{
    public class ApplicationDbContextDataSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Add roles supported
            await roleManager.CreateAsync(new IdentityRole(ApplicationIdentityConstants.Roles.Administrator));
            await roleManager.CreateAsync(new IdentityRole(ApplicationIdentityConstants.Roles.Config));
            await roleManager.CreateAsync(new IdentityRole(ApplicationIdentityConstants.Roles.Member));

            // New admin user
            string adminUserName = "cleaner";
            var adminUser = new ApplicationUser
            {
                UserName = adminUserName,
                Email = adminUserName,
                IsDown = true,
                EmailConfirmed = true,
                IsFirstLogin = false
            };

            // Add new user and their role
            await userManager.CreateAsync(adminUser, ApplicationIdentityConstants.DefaultPassword);
            adminUser = await userManager.FindByNameAsync(adminUserName);
            await userManager.AddToRoleAsync(adminUser, ApplicationIdentityConstants.Roles.Administrator);
            await userManager.AddToRoleAsync(adminUser, ApplicationIdentityConstants.Roles.Config);
            await userManager.AddToRoleAsync(adminUser, ApplicationIdentityConstants.Roles.Member);
        }
    }
}
