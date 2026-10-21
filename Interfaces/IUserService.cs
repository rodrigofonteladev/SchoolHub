using SchoolHub.Common;
using SchoolHub.Common.User;
using SchoolHub.Models.Entities;

namespace SchoolHub.Interfaces
{
    public interface IUserService
    {
        Task<Result<CreateUserResult>> CreateUserAsync(string userName, string email, string phoneNumber);
        Task<Result<string>> GeneratePasswordResetLinkAsync(ApplicationUser user);
    }
}
