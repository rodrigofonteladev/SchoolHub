using SchoolHub.Models.Entities;

namespace SchoolHub.Common.User
{
    public class CreateUserResult
    {
        public ApplicationUser User { get; set; } = null!;
    }
}