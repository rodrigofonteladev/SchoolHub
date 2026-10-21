using Microsoft.AspNetCore.Identity;
using SchoolHub.Common.Enums;

namespace SchoolHub.SeedDatabase;

public static class SeedRole
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var roleName in Enum.GetNames<Role>())
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Could not create role '{roleName}': {string.Join(", ", result.Errors.Select(error => error.Description))}");
                }
            }
        }
    }
}
