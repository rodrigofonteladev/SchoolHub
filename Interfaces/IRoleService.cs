using SchoolHub.Common;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.Role;

namespace SchoolHub.Interfaces
{
    public interface IRoleService
    {
        Task<List<ListRoleViewModel>> GetAllAsync();
        Task<Result> CreateRoleAsync(string role);
        Task<Result> AssignRoleAsync(ApplicationUser user, string role);
    }
}