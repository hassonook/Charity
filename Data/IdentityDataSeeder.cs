using Charity.Models;
using Microsoft.AspNetCore.Identity;

namespace Charity.Data
{
    public static class IdentityDataSeeder<TIdentityUser, TIdentityRole>
        where TIdentityUser : ApplicationUser, new()
        where TIdentityRole : IdentityRole, new()
    {
        private static string[] roles = { "Admin", "User" };
        private const string DefaultAdminUserEmail = "admin@charity.com";
        private const string DefaultAdminUserName = "admin";
        private const string DefaultAdminUserPassword = "admin";

        private static async Task CreateDefaultAdminRole(RoleManager<TIdentityRole> roleManager)
        {
            // Make sure we have an Administrator role
                foreach (var item in roles)
                {
                    var roleExist = await roleManager.RoleExistsAsync(item);
                    if (!roleExist)
                    {
                        var roleResult = await roleManager.CreateAsync(new TIdentityRole { Name = item });

                        if (!roleResult.Succeeded)
                        {
                            throw new ApplicationException($"Could not create '{item}' role");
                        }
                    }
                }
        }

        private static async Task<ApplicationUser> CreateDefaultAdminUser(UserManager<ApplicationUser> userManager)
        {
            var user = await userManager.FindByEmailAsync(DefaultAdminUserEmail);
            if (user == null)
            {
                user = new TIdentityUser
                {
                    UserName = DefaultAdminUserName,
                    Email = DefaultAdminUserEmail,
                    EmailConfirmed = true,
                };
                var userResult = await userManager.CreateAsync(user, DefaultAdminUserPassword);

                if (!userResult.Succeeded)
                {
                    throw new ApplicationException($"Could not create '{DefaultAdminUserEmail}' user");
                }
            }

            return user;
        }

        private static async Task AddDefaultAdminRoleToDefaultAdminUser(
            UserManager<ApplicationUser> userManager,
            ApplicationUser user)
        {
            // Add user to Administrator role if it's not already associated
            foreach (var item in roles)
            {
                if (!(await userManager.GetRolesAsync(user)).Contains(item))
                {
                    var addToRoleResult = await userManager.AddToRoleAsync(user, item);
                    if (!addToRoleResult.Succeeded)
                    {
                        throw new ApplicationException(
                            $"Could not add user '{DefaultAdminUserEmail}' to '{item}' role");
                    }
                }
            }
        }

        public static async Task SeedDataAsync(IServiceProvider services, ILogger logger)
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var roleManager = services.GetRequiredService<RoleManager<TIdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            var retries = 3;
        ensureCreated:
            try
            {
                await context.Database.EnsureCreatedAsync();
            }
            catch (Exception ex)
            {
                if (retries == 0)
                {
                    throw;
                }

                logger.LogWarning(ex, $"An error occurred while seeding the database ; maybe the server isn't currently running, will retry {retries} more times after 5 seconds");
                await Task.Delay(5000);

                retries -= 1;
                goto ensureCreated;
            }

            await CreateDefaultAdminRole(roleManager);
            var defaultAdminUser = await CreateDefaultAdminUser(userManager);
            await AddDefaultAdminRoleToDefaultAdminUser(userManager, defaultAdminUser);
        }
    }
}