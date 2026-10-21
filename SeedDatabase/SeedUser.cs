using Microsoft.AspNetCore.Identity;
using SchoolHub.Common.Enums;
using SchoolHub.Models.Entities;

namespace SchoolHub.SeedDatabase;

public static class SeedUser
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        using var scope = services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("SeedUser");

        var userName = configuration["SeedAdminUser:UserName"];
        var email = configuration["SeedAdminUser:Email"];
        var password = configuration["SeedAdminUser:Password"];

        if (string.IsNullOrWhiteSpace(userName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning(
                "Admin user seed skipped. Configure SeedAdminUser:UserName, SeedAdminUser:Email and SeedAdminUser:Password.");
            return;
        }

        var user = await userManager.FindByNameAsync(userName);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true,
                IsActive = true
            };

            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Could not create seed admin user: {string.Join(", ", createResult.Errors.Select(error => error.Description))}");
            }

            logger.LogInformation("Seed admin user {UserName} created.", userName);
        }
        else if (!user.IsActive)
        {
            user.IsActive = true;
            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Could not activate seed admin user: {string.Join(", ", updateResult.Errors.Select(error => error.Description))}");
            }
        }

        if (!await userManager.IsInRoleAsync(user, Role.Admin.ToString()))
        {
            var roleResult = await userManager.AddToRoleAsync(user, Role.Admin.ToString());
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Could not assign the Admin role to seed user: {string.Join(", ", roleResult.Errors.Select(error => error.Description))}");
            }
        }
    }
}
