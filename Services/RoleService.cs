using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolHub.Common;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.Role;

namespace SchoolHub.Services
{
    public class RoleService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager) : IRoleService
    {
        public async Task<List<ListRoleViewModel>> GetAllAsync()
        {
            return await roleManager.Roles
                .AsNoTracking()
                .OrderBy(role => role.Name)
                .Select(role => new ListRoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name!,
                    NormalizedName = role.NormalizedName!
                })
                .ToListAsync();
        }

        public async Task<Result> CreateRoleAsync(string role)
        {
            var result = await roleManager.CreateAsync(new IdentityRole(role));
            if (!result.Succeeded)
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result.Failure(errors);
            }

            return Result.Success();
        }

        public async Task<Result> AssignRoleAsync(ApplicationUser user, string role)
        {
            var result = await userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result.Failure(errors);
            }

            return Result.Success();
        }
    }
}