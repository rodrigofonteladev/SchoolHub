using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "El usuario o correo electrónico es requerido")]
    [Display(Name = "Usuario o correo electrónico")]
    public string UserNameOrEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Recordarme")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}
