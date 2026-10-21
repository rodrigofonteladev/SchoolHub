using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.DocumentType
{
    public class RegisterDocumentTypeViewModel
    {
        [Required(ErrorMessage = "El nombre del tipo de documento es requerido")]
        [StringLength(50)]
        [Display(Name = "Tipo de documento")]
        public string Name { get; set; } = null!;

        [Display(Name = "Activo")]
        public bool IsActive { get; set; } = true;
    }
}