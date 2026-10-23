using SchoolHub.Models.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.Teacher
{
    public class RegisterTeacherViewModel : RegisterPersonViewModel
    {
        [Required(ErrorMessage = "El tipo de documento es requerido")]
        [Display(Name = "Tipo de Documento")]
        public int? DocumentTypeId { get; set; }

        public IEnumerable<SelectListItem> DocumentTypes { get; set; } = new List<SelectListItem>();
    }
}
