using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.Role
{
    public class RegisterRoleViewModel
    {
        [Required(ErrorMessage = "The name role is required")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; } = null!;
    }
}