using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;

namespace SchoolHub.Services
{
    public class AssignUserService(AppDbContext context) : IAssignUserService
    {
        public async Task CreateAssignUserAccountAsync(string userCode, int personId, string userId)
        {
            var userAccount = new UserAccount
            {
                UserCode = userCode,
                StudentId = personId,
                ApplicationUserId = userId
            };

            await context.UserAccounts.AddAsync(userAccount);
            await context.SaveChangesAsync();
        }
    }
}