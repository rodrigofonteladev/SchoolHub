using Microsoft.AspNetCore.Identity;

namespace SchoolHub.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; } = false;

        public UserAccount UserAccount { get; set; } = null!;
    }
}
