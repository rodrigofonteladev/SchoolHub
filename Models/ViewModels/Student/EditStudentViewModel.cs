using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.Student
{
    public class EditStudentViewModel
    {
        [Required] public string FullName { get; set; } = null!;
        [Required] public string PaternalSurname { get; set; } = null!;
        [Required] public string MaternalSurname { get; set; } = null!;
        [Required, RegularExpression(@"^\d+$")] public string DocumentNumber { get; set; } = null!;
        [Required, EmailAddress] public string Email { get; set; } = null!;
        [Required] public string Address { get; set; } = null!;
        [Required] public DateOnly? Birthday { get; set; }
        [Required] public int? DocumentTypeId { get; set; }
        public IEnumerable<SelectListItem> DocumentTypes { get; set; } = new List<SelectListItem>();
    }
}
